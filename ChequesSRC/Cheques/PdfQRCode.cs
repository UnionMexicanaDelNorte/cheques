/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfQRCode
//	Display QR Code as image resource.
//
//	Granotech Limited
//	Author: Uzi Granot
//	Version: 1.0
//	Date: April 1, 2013
//	Copyright (C) 2013-2015 Granotech Limited. All Rights Reserved
//
//	PdfFileWriter C# class library and TestPdfFileWriter test/demo
//  application are free software.
//	They is distributed under the Code Project Open License (CPOL).
//	The document PdfFileWriterReadmeAndLicense.pdf contained within
//	the distribution specify the license agreement and other
//	conditions and notes. You must read this document and agree
//	with the conditions specified in order to use this software.
//
//	For version history please refer to PdfDocument.cs
//
/////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/// <summary>
/// QR Code error correction code enumeration
/// </summary>
public enum ErrorCorrection
	{
	/// <summary>
	/// Low
	/// </summary>
	L,

	/// <summary>
	/// Medium
	/// </summary>
	M,

	/// <summary>
	/// Medium-high
	/// </summary>
	Q,

	/// <summary>
	/// High
	/// </summary>
	H
	};

/// <summary>
/// PDF QR Code resource class
/// </summary>
/// <remarks>
/// <para>
/// The QR Code object is a PDF Image object.
/// </para>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#QRCodeSupport">2.8 QR Code Support</a>
/// </para>
/// </remarks>
public class PdfQRCode : PdfObject
	{
	/// <summary>
	/// Gets matrix dimension.
	/// </summary>
	public Int32 MatrixDimension {get; private set;}

	/// <summary>
	/// Gets module size in pixels
	/// </summary>
	public Int32 ModuleSize {get; set;}

	/// <summary>
	/// Gets quiet zone in pixels.
	/// </summary>
	public Int32 QuietZone {get; set;}

	/// <summary>
	/// Segment marker
	/// </summary>
	public const Char SegmentMarker = (Char) 256;

	private Byte[,] QRCodeMatrix;
	private PdfObject ImageLengthObject;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF QR Code constructor
	/// </summary>
	/// <param name="Document">Parent PDF document.</param>
	/// <param name="DataString">Data string to encode.</param>
	/// <param name="ErrorCorrection">Error correction code.</param>
	////////////////////////////////////////////////////////////////////
	public PdfQRCode
			(
			PdfDocument		Document,
			String			DataString,
			ErrorCorrection	ErrorCorrection
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create QR Code object
		QREncoder Encoder = new QREncoder();
		QRCodeMatrix = Encoder.EncodeQRCode(DataString, ErrorCorrection);
		MatrixDimension = Encoder.MatrixDimension;

		// create stream length object
		ImageLengthObject = new PdfObject(Document, ObjectType.Other);
		Dictionary.AddIndirectReference("/Length", ImageLengthObject);

		// set default values
		ModuleSize = 4;
		QuietZone = 16;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF QR Code constructor
	/// </summary>
	/// <param name="Document">Parent PDF document.</param>
	/// <param name="SegDataString">Data string array to encode.</param>
	/// <param name="ErrorCorrection">Error correction code.</param>
	/// <remarks>
	/// The program will calculate the best encoding mode for each segment.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfQRCode
			(
			PdfDocument		Document,
			String[]		SegDataString,
			ErrorCorrection	ErrorCorrection
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create QR Code object
		QREncoder Encoder = new QREncoder();
		QRCodeMatrix = Encoder.EncodeQRCode(SegDataString, ErrorCorrection);
		MatrixDimension = Encoder.MatrixDimension;

		// create stream length object
		ImageLengthObject = new PdfObject(Document, ObjectType.Other);
		Dictionary.AddIndirectReference("/Length", ImageLengthObject);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Gets image side in pixels.
	/// </summary>
	////////////////////////////////////////////////////////////////////
	public Int32 ImageSide
		{
		get
			{
			return(MatrixDimension * ModuleSize + 2 * QuietZone);
			}
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// shortcut
		PdfBinaryWriter PdfFile = Document.PdfFile;

		// add items to dictionary
		Dictionary.Add("/Subtype", "/Image");
		Dictionary.AddInteger("/Width", ImageSide);
		Dictionary.AddInteger("/Height", ImageSide);
		Dictionary.Add("/Filter", "/DCTDecode");
		Dictionary.Add("/ColorSpace", "/DeviceRGB");
		Dictionary.Add("/BitsPerComponent", "8");

		// write dictionary
		Dictionary.WriteToPdfFile();

		// output stream
		PdfFile.WriteString("stream\n");

		// save pdf file position
		Int64 streamStart = PdfFile.BaseStream.Position;

		// debug
		if(Document.Debug)
			{
			PdfFile.WriteString("*** IMAGE PLACE HOLDER ***");
			}

		// copy image file to output file
		else
			{
			// create bitmap image
			Bitmap Image = QRCodeImage();

			// create memory stream
			MemoryStream MS = new MemoryStream();

			// save image to memory stream
			Image.Save(MS, ImageFormat.Jpeg);

			// image byte array
			Byte[] ByteContents = MS.GetBuffer();

			// encryption
			if(Document.Encryption != null) ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

			// write memory stream internal buffer to PDF file
			PdfFile.Write(ByteContents);

			// close and dispose memory stream
			MS.Close();

			// dispose bitmap resources
			Image.Dispose();
			}

		// save stream length
		ImageLengthObject.ContentsString.Append(((Int32) (PdfFile.BaseStream.Position - streamStart)).ToString());

		// output stream
		PdfFile.WriteString("\nendstream\nendobj\n");
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Convert QRCode boolean matrix into QRCode image
	////////////////////////////////////////////////////////////////////

	private Bitmap QRCodeImage()
		{
		// create white and black brushes
		SolidBrush BrushWhite = new SolidBrush(Color.White);
		SolidBrush BrushBlack = new SolidBrush(Color.Black);

		// create picture object and make it white
		Int32 PictureSide = ImageSide;
		Bitmap Picture = new Bitmap(PictureSide, PictureSide);
		Graphics Graphics = Graphics.FromImage(Picture);
		Graphics.FillRectangle(BrushWhite, 0, 0, PictureSide, PictureSide);

		// paint QR Code image
		for(Int32 Row = 0; Row < MatrixDimension; Row++) for(Int32 Col = 0; Col < MatrixDimension; Col++)
			{
			if((QRCodeMatrix[Row, Col] & 1) != 0) Graphics.FillRectangle(BrushBlack, QuietZone + Col * ModuleSize, QuietZone + Row * ModuleSize, ModuleSize, ModuleSize);
			}
		Graphics.Dispose();
		return(Picture);
		}
	}
}
