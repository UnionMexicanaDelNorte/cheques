/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfAxialShading
//	PDF Axial shading indirect object.
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
////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF axial shading resource class
/// </summary>
/// <remarks>
/// Derived class from PdfObject
/// </remarks>
////////////////////////////////////////////////////////////////////
public class PdfAxialShading : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF axial shading constructor.
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <param name="PosX">Position X</param>
	/// <param name="PosY">Position Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <param name="ShadingFunction">Shading function</param>
	////////////////////////////////////////////////////////////////////
	public PdfAxialShading
			(
			PdfDocument		Document,
			Double			PosX,
			Double			PosY,
			Double			Width,
			Double			Height,
			PdfShadingFunction	ShadingFunction
			) : base(Document)
		{
		// create resource code
		ResourceCode = Document.GenerateResourceNumber('S');

		// color space red, green and blue
		Dictionary.Add("/ColorSpace", "/DeviceRGB");

		// shading type axial
		Dictionary.Add("/ShadingType", "2");

		// bounding box
		Dictionary.AddRectangle("/BBox", PosX, PosY, PosX + Width, PosY + Height);

		// assume the direction of color change is along x axis
		Dictionary.AddRectangle("/Coords", PosX, PosY, PosX + Width, PosY);

		// add shading function to shading dictionary
		Dictionary.AddIndirectReference("/Function", ShadingFunction);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Set Axis direction
	/// </summary>
	/// <param name="PosX">Position X</param>
	/// <param name="PosY">Position Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	////////////////////////////////////////////////////////////////////
	public void SetAxis
			(
			Double	PosX,
			Double	PosY,
			Double	Width,
			Double	Height
			)
		{
		Dictionary.AddRectangle("/Coords", PosX, PosY, PosX + Width, PosY + Height);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Sets anti-alias parameter
	/// </summary>
	/// <param name="Value">Anti-alias true or false</param>
	////////////////////////////////////////////////////////////////////
	public void AntiAlias
			(
			Boolean		Value
			)
		{
		Dictionary.AddBoolean("/AntiAlias", Value); 
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Extend shading beyond axis
	/// </summary>
	/// <param name="Before">Before (true or false)</param>
	/// <param name="After">After (true or false)</param>
	////////////////////////////////////////////////////////////////////
	public void ExtendShading
			(
			Boolean		Before,
			Boolean		After
			)
		{
		Dictionary.AddFormat("/Extend", "[{0} {1}]", Before ? "true" : "false", After ? "true" : "false"); 
		return;
		}
	}
}
