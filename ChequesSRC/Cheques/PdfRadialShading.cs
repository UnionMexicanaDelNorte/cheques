/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfRadialShading
//	PDF radial shading resource class.
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
/// PDF radial shading resource class
/// </summary>
/// <remarks>
/// Derived class from PdfObject
/// </remarks>
public class PdfRadialShading : PdfObject
	{
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF radial shading constructor.
	/// </summary>
	/// <param name="Document">Parent PDF document object</param>
	/// <param name="PosX">Position X</param>
	/// <param name="PosY">Position Y</param>
	/// <param name="Width">Width</param>
	/// <param name="Height">Height</param>
	/// <param name="ShadingFunction">Shading function</param>
	////////////////////////////////////////////////////////////////////
	public PdfRadialShading
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

		// shading type radial
		Dictionary.Add("/ShadingType", "3");

		// bounding box
		Dictionary.AddRectangle("/BBox", PosX, PosY, PosX + Width, PosY + Height);

		// set center to bounding box center and radius to half the diagonal
		Dictionary.AddFormat("/Coords", "[{0} {1} {2} {0} {1} 0]", ToPt(PosX + Width / 2), ToPt(PosY + Height / 2), ToPt(Math.Sqrt(Width * Width + Height * Height) / 2));

		// add shading function to shading dictionary
		Dictionary.AddIndirectReference("/Function", ShadingFunction);
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Set circle
	/// </summary>
	/// <param name="PosX">Centre position X</param>
	/// <param name="PosY">Centre position Y</param>
	/// <param name="Radius">Radius</param>
	////////////////////////////////////////////////////////////////////
	public void SetCircle
			(
			Double	PosX,
			Double	PosY,
			Double	Radius
			)
		{
		Dictionary.AddFormat("/Coords", "[{0} {1} {2} {0} {1} 0]", ToPt(PosX), ToPt(PosY), ToPt(Radius));
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Set circles
	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Set two circles
	/// </summary>
	/// <param name="PosX0">Centre position0 X</param>
	/// <param name="PosY0">Centre position0 Y</param>
	/// <param name="Radius0">Radius0</param>
	/// <param name="PosX1">Centre position1 X</param>
	/// <param name="PosY1">Centre position1 Y</param>
	/// <param name="Radius1">Radius1</param>
	public void SetCircles
			(
			Double	PosX0,
			Double	PosY0,
			Double	Radius0,
			Double	PosX1,
			Double	PosY1,
			Double	Radius1
			)
		{
		Dictionary.AddFormat("/Coords", "[{0} {1} {2} {3} {4} {5}]", ToPt(PosX0), ToPt(PosY0), ToPt(Radius0), ToPt(PosX1), ToPt(PosY1), ToPt(Radius1));
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
