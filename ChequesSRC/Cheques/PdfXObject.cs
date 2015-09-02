/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfXObject
//	PDF X Object resource class.
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

namespace PdfFileWriter
{
/// <summary>
/// PDF X object resource class
/// </summary>
public class PdfXObject : PdfContents
	{
	/// <summary>
	/// Bounding box rectangle
	/// </summary>
	public PdfRectangle Rect {get; set;}

	/// <summary>
	/// Bounding box left side
	/// </summary>
	public Double Left
		{
		get {return(Rect.Left);}
		set {Rect.Left = value;}
		}

	/// <summary>
	/// Bounding box bottom side
	/// </summary>
	public Double Bottom
		{
		get {return(Rect.Bottom);}
		set {Rect.Bottom = value;}
		}

	/// <summary>
	/// Bounding box right side
	/// </summary>
	public Double Right
		{
		get {return(Rect.Right);}
		set {Rect.Right = value;}
		}

	/// <summary>
	/// Bounding box top side
	/// </summary>
	public Double Top
		{
		get {return(Rect.Top);}
		set {Rect.Top = value;}
		}

	/// <summary>
	/// PDF X Object constructor
	/// </summary>
	/// <param name="Document">PDF document</param>
	/// <param name="Width">X Object width</param>
	/// <param name="Height">X Object height</param>
	public PdfXObject
			(
			PdfDocument		Document,
			Double			Width = 1.0,
			Double			Height = 1.0
			) : base(Document, "/XObject")
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('X');

		// add subtype to dictionary
		Dictionary.Add("/Subtype", "/Form");

		// set boundig box rectangle
		Rect = new PdfRectangle(0.0, 0.0, Width, Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// bounding box
		Dictionary.AddRectangle("/BBox", Rect);

		// call PdfObject base routine
		base.WriteObjectToPdfFile();

		// exit
		return;
		}
	}
}
