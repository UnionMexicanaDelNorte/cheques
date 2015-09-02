/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfChart
//	Display charts in the PDF document.
//  Charts are displayed as images.
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
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
using System.Text;

namespace PdfFileWriter
{
/// <summary>
/// Font size units for PdfChart.CreateFont method enumeration
/// </summary>
public enum FontSizeUnit
	{
	/// <summary>
	/// Pixel
	/// </summary>
	Pixel,

	/// <summary>
	/// Point
	/// </summary>
	Point,

	/// <summary>
	/// PDF document user unit
	/// </summary>
	UserUnit,

	/// <summary>
	/// Inch
	/// </summary>
	Inch,

	/// <summary>
	/// CM
	/// </summary>
	cm,

	/// <summary>
	/// MM
	/// </summary>
	mm
	}

/// <summary>
/// PDF chart resource class
/// </summary>
/// <remarks>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#ChartingSupport">2.10 Charting Support</a>
/// </para>
/// <para>
/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#DrawChart">For example of drawing image see 3.11. Draw Pie Chart</a>
/// </para>
/// </remarks>
public class PdfChart : PdfObject
	{
	/// <summary>
	/// Chart object (.NET).
	/// </summary>
	public Chart Chart {get; private set;}			// chart object

	/// <summary>
	/// Chart width in user units.
	/// </summary>
	public Double Width {get; private set;}			// width in user units

	/// <summary>
	/// Chart height in user units.
	/// </summary>
	public Double Height {get; private set;}			// height in user units

	/// <summary>
	/// Chart resolution in pixels per inch.
	/// </summary>
	public Double Resolution {get; private set;}		// resolution in pixels per inch

	/// <summary>
	/// Chart width in pixels.
	/// </summary>
	public Int32 WidthPix {get { return(Chart.Width); }}		// width in pxels

	/// <summary>
	/// Chart height in pixels.
	/// </summary>
	public Int32 HeightPix {get { return(Chart.Height); }}	// height in pixels

	private  PdfObject	ImageLengthObject;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF chart constructor
	/// </summary>
	/// <param name="Document">Document object parent of this chart.</param>
	/// <param name="Chart">.NET Chart object.</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional argument)</param>
	////////////////////////////////////////////////////////////////////
	public PdfChart
			(
			PdfDocument		Document,
			Chart			Chart,
			Double			Resolution = 0.0	// pixels per inch
			) : base(Document, ObjectType.Stream, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// create stream length object
		ImageLengthObject = new PdfObject(Document, ObjectType.Other);
		Dictionary.AddIndirectReference("/Length", ImageLengthObject);

		// save chart
		this.Chart = Chart;

		// save resolution
		if(Resolution != 0)
			{
			// chart resolution in pixels per inch
			this.Resolution = Resolution;
			this.Chart.RenderingDpiY = Resolution;
			}
		else
			{
			this.Resolution = this.Chart.RenderingDpiY;
			}

		// calculate chart size in user coordinates
		Width = (Double) this.Chart.Width * 72.0 / (this.Resolution * Document.ScaleFactor);
		Height = (Double) this.Chart.Height * 72.0 / (this.Resolution * Document.ScaleFactor);

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Save chart to PDF output file
	/// </summary>
	/// <param name="DisposeChart">Dispose .NET chart object (see remarks.)</param>
	/// <remarks>
	/// <para>
	/// Call this method after chart is fully constructed.
	/// Calling this method flush the resource to the output file.
	/// It reduces the unmanaged memory requirements.
	/// If method is not called, the resource will be sent to the
	/// output file when Document.CreateFile() method is called.
	/// </para>
	/// <para>
	/// If DisposeChart is true (by default it is true), this method
	/// will call .NET Chart.Dispose() method to free unmanaged
	/// resources. If DisposeChart is false, it is the responsibility
	/// of the caller to free the resources.
	/// </para>
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public void SaveToPdfFile
			(
			Boolean		DisposeChart = true
			)
		{
		// write to output file
		WriteObjectHeaderToPdfFile();
		if(DisposeChart) Chart.Dispose();
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Static method to create .NET Chart object.
	/// </summary>
	/// <param name="Document">Current document object.</param>
	/// <param name="Width">Chart width in user units.</param>
	/// <param name="Height">Chart height in user units.</param>
	/// <param name="Resolution">Resolution in pixels per inch (optional argument).</param>
	/// <returns>.NET Chart object</returns>
	/// <remarks>
	/// The returned chart has the correct width and height in pixels.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public static Chart CreateChart
			(
			PdfDocument		Document,
			Double			Width,
			Double			Height,
			Double			Resolution = 0.0
			)
		{
		// create chart
		Chart Chart = new Chart();

		// save resolution
		if(Resolution != 0) Chart.RenderingDpiY = Resolution;

		// image size in pixels
		Chart.Width = (Int32) (Chart.RenderingDpiY * Width * Document.ScaleFactor / 72.0 + 0.5);
		Chart.Height = (Int32) (Chart.RenderingDpiY * Height * Document.ScaleFactor / 72.0 + 0.5);

		// return chart
		return(Chart);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Helper method to create a font for chart drawing.
	/// </summary>
	/// <param name="FontFamilyName">Font family name.</param>
	/// <param name="FontStyle">Font style.</param>
	/// <param name="FontSize">Font size per unit argument.</param>
	/// <param name="Unit">Font size unit.</param>
	/// <returns>.NET font</returns>
	////////////////////////////////////////////////////////////////////
	public Font CreateFont
			(
			String		FontFamilyName,	// font family name
			FontStyle	FontStyle,		// font style
			Double		FontSize,		// as per units below
			FontSizeUnit Unit			// unit of measure
			)
		{
		// calculate size
		Int32 SizeInPixels = 0;
		switch(Unit)
			{
			case FontSizeUnit.Pixel:
				SizeInPixels = (Int32) (FontSize + 0.5);
				break;

			case FontSizeUnit.Point:
				SizeInPixels = (Int32) (FontSize * Resolution / 72.0 + 0.5);
				break;

			case FontSizeUnit.UserUnit:
				SizeInPixels = (Int32) (FontSize * Resolution * Document.ScaleFactor / 72.0 + 0.5);
				break;

			case FontSizeUnit.Inch:
				SizeInPixels = (Int32) (FontSize * Resolution + 0.5);
				break;

			case FontSizeUnit.cm:
				SizeInPixels = (Int32) (FontSize * Resolution / 2.54 + 0.5);
				break;

			case FontSizeUnit.mm:
				SizeInPixels = (Int32) (FontSize * Resolution / 25.4 + 0.5);
				break;
			}

		// create font
		return(new Font(FontFamilyName, SizeInPixels, FontStyle, GraphicsUnit.Pixel));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates chart size to preserve aspect ratio.
	/// </summary>
	/// <param name="InputSize">Chart display area.</param>
	/// <returns>Adjusted chart display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			SizeD InputSize
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, InputSize.Width, InputSize.Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates chart size to preserve aspect ratio.
	/// </summary>
	/// <param name="Width">Chart display width.</param>
	/// <param name="Height">Chart display height.</param>
	/// <returns>Adjusted chart display area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public SizeD ImageSize
			(
			Double	Width,
			Double	Height
			)
		{
		return(ImageSizePos.ImageSize(WidthPix, HeightPix, Width, Height));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates chart size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="InputSize">Chart display area</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted chart size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfRectangle ImageSizePosition
			(
			SizeD				InputSize,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0, InputSize.Width, InputSize.Height, Alignment));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Calculates chart size to preserve aspect ratio and sets position.
	/// </summary>
	/// <param name="Width">Chart display width.</param>
	/// <param name="Height">Chart display height.</param>
	/// <param name="Alignment">Content alignment</param>
	/// <returns>Adjusted chart size and position within area.</returns>
	/// <remarks>
	/// Calculates best fit to preserve aspect ratio and adjust
	/// position according to content alignment argument.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public PdfRectangle ImageSizePosition
			(
			Double				Width,
			Double				Height,
			ContentAlignment	Alignment
			)
		{
		return(ImageSizePos.ImageArea(WidthPix, HeightPix, 0.0, 0.0, Width, Height, Alignment));
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
		Dictionary.AddInteger("/Width", Chart.Width);
		Dictionary.AddInteger("/Height", Chart.Height);
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
			PdfFile.WriteString("*** CHART PLACE HOLDER ***");
			}

		// copy image file to output file
		else
			{
			// create memory stream
			MemoryStream MS = new MemoryStream();

			// save image to memory stream
			Chart.SaveImage(MS, ChartImageFormat.Jpeg);

			// image byte array
			Byte[] ByteContents = MS.GetBuffer();

			// encryption
			if(Document.Encryption != null) ByteContents = Document.Encryption.EncryptByteArray(ObjectNumber, ByteContents);

			// write memory stream internal buffer to PDF file
			PdfFile.Write(ByteContents);

			// close and dispose memory stream
			MS.Close();
			}

		// save stream length
		ImageLengthObject.ContentsString.Append(((Int32) (PdfFile.BaseStream.Position - streamStart)).ToString());

		// output stream
		PdfFile.WriteString("\nendstream\nendobj\n");
		return;
		}
	}
}
