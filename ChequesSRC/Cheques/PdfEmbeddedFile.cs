/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfEmbeddedFile
//	PDF embedded file class. 
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
using System.IO;

namespace PdfFileWriter
{
/// <summary>
/// PDF Embedded file class
/// </summary>
public class PdfEmbeddedFile : PdfObject
	{
	/// <summary>
	/// Gets file name
	/// </summary>
	public String FileName {get; private set;}

	/// <summary>
	/// Gets Mime type
	/// </summary>
	/// <remarks>
	/// <para>
	/// The PDF embedded file translates the file extension into mime type string.
	/// If the translation fails the MimeType is set to null.
	/// </para>
	/// </remarks>
	public String MimeType {get; private set;}

	/// <summary>
	/// PDF embedded file class constructor
	/// </summary>
	/// <param name="Document">Current document</param>
	/// <param name="FileName">File name</param>
	/// <param name="PdfFileName">PDF file name (see remarks)</param>
	/// <remarks>
	/// <para>
	/// FileName is the name of the source file on the disk.
	/// PDFFileName is the name of the as saved within the PDF document file.
	/// If PDFFileName is not given or it is set to null, the class takes
	/// the disk's file name without the path.
	/// </para>
	/// </remarks>
	public PdfEmbeddedFile
			(
			PdfDocument		Document,
			String			FileName,
			String			PdfFileName = null
			) : base(Document, ObjectType.Dictionary, "/Filespec")
		{
		// save file name
		this.FileName = FileName;

		// test exitance
		if(!File.Exists(FileName)) throw new ApplicationException("Embedded file " + FileName + " does not exist");

		// get file length
		FileInfo FI = new FileInfo(FileName);
		if(FI.Length > Int32.MaxValue - 4095) throw new ApplicationException("Embedded file " + FileName + " too long");
		Int32 FileLength = (Int32) FI.Length;

		// translate file extension to mime type string
		MimeType = ExtToMime.TranslateExtToMime(FI.Extension);

		// create embedded file object
		PdfObject EmbeddedFile = new PdfObject(Document, ObjectType.Stream, "/EmbeddedFile");

		// save uncompressed file length
		EmbeddedFile.Dictionary.AddFormat("/Params", "<</Size {0}>>", FileLength);

		// file data content byte array
		Byte[] FileData = new Byte[FileLength];

		// load all the file's data
		FileStream DataStream = null;
		try
			{
			// open the file
			DataStream = new FileStream(FileName, FileMode.Open, FileAccess.Read);

			// read all the file
			if(DataStream.Read(FileData, 0, FileLength) != FileLength) throw new Exception();
			}

		// loading file failed
		catch(Exception)
			{
			throw new ApplicationException("Invalid media file: " + FileName);
			}

		// close the file
		DataStream.Close();

		// compress the data
		Byte[] FileDataComp = CompressStream(FileData);
		if(FileDataComp != null)
			{
			FileData = FileDataComp;
			EmbeddedFile.Dictionary.Add("/Filter", "/FlateDecode");
			}

		// encryption
		if(Document.Encryption != null) FileData = Document.Encryption.EncryptByteArray(EmbeddedFile.ObjectNumber, FileData);

		// add compressed file length
		EmbeddedFile.Dictionary.AddInteger("/Length", FileData.Length);

		// shortcut
		PdfBinaryWriter PdfFile = Document.PdfFile;

		// save file position for this object
		EmbeddedFile.FilePosition = PdfFile.BaseStream.Position;

		// write object header
		PdfFile.WriteFormat("{0} 0 obj\n", EmbeddedFile.ObjectNumber);

		// write dictionary
		EmbeddedFile.Dictionary.WriteToPdfFile();

		// output stream
		PdfFile.WriteString("stream\n");

        // debug
		if(Document.Debug)
			{
			PdfFile.WriteString("*** MEDIAFILE PLACE HOLDER ***");
			}

		// output embedded font
		else
			{
			PdfFile.Write(FileData);
			}

		// output stream
		PdfFile.WriteString("\nendstream\n");

		// output object trailer
		PdfFile.WriteString("endobj\n");

		// file spec object type
		Dictionary.Add("/Type", "/Filespec");

		// PDF file name
		if(String.IsNullOrWhiteSpace(PdfFileName)) PdfFileName = FI.Name;
		Dictionary.AddPdfString("/F", PdfFileName);
		Dictionary.AddPdfString("/UF", PdfFileName);

		// add reference
		Dictionary.AddFormat("/EF", "<</F {0} 0 R /UF {0} 0 R>>", EmbeddedFile.ObjectNumber);
		return;
		}
	}

internal class ExtToMime : IComparable<ExtToMime>
	{
	internal String	Ext;
	internal String	Mime;

	internal ExtToMime
			(
			String	Ext,
			String	Mime
			)
		{
		this.Ext = Ext;
		this.Mime = Mime;
		return;
		}

	internal static String TranslateExtToMime
			(
			String Ext
			)
		{
		Int32 Index = Array.BinarySearch(ExtToMimeArray, new ExtToMime(Ext, null));
		return(Index >= 0 ? ExtToMimeArray[Index].Mime : null);
		}

	/// <summary>
	/// Compare ExtToMime records
	/// </summary>
	/// <param name="Other">Other record</param>
	/// <returns></returns>
	public Int32 CompareTo
			(
			ExtToMime Other
			)
		{
		return(String.Compare(this.Ext, Other.Ext, true));
		}

	private static ExtToMime[]	ExtToMimeArray =
		{
		new ExtToMime(".avi", "video/avi"),			// Covers most Windows-compatible formats including .avi and .divx
		new ExtToMime(".divx", "video/avi"),		// Covers most Windows-compatible formats including .avi and .divx
		new ExtToMime(".mpg", "video/mpeg"),		// MPEG-1 video with multiplexed audio; Defined in RFC 2045 and RFC 2046
		new ExtToMime(".mpeg", "video/mpeg"),		// MPEG-1 video with multiplexed audio; Defined in RFC 2045 and RFC 2046
		new ExtToMime(".mp4", "video/mp4"),			// MP4 video; Defined in RFC 4337
		new ExtToMime(".mov", "video/quicktime"),	// QuickTime video .mov
		new ExtToMime(".wav", "audio/wav"),			// audio
		new ExtToMime(".wma", "audio/x-ms-wma"),	// audio
		new ExtToMime(".mp3", "audio/mpeg"),		// audio
		};

	static ExtToMime()
		{
		Array.Sort(ExtToMimeArray);
		return;
		}
	}
}
