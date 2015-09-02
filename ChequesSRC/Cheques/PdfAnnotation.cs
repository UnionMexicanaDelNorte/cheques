/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfAnnotation
//	PDF Annotation class. 
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

namespace PdfFileWriter
{
/// <summary>
/// File attachement icon
/// </summary>
public enum FileAttachIcon
	{
	/// <summary>
	/// Graph
	/// </summary>
	Graph,

	/// <summary>
	/// Paperclip
	/// </summary>
	Paperclip,

	/// <summary>
	/// PushPin (default)
	/// </summary>
	PushPin,

	/// <summary>
	/// Tag
	/// </summary>
	Tag,
	}

/// <summary>
/// PDF Annotation class
/// </summary>
public class PdfAnnotation : PdfObject
	{
	// screen annotation
	internal PdfDisplayMedia DisplayMedia;

	// annotation rectangle
	internal PdfRectangle AnnotRect;

	/// <summary>
	/// PDF link annotation constructor
	/// </summary>
	/// <param name="Page">Associated page</param>
	/// <param name="AnnotRect">Annotation rectangle</param>
	/// <param name="WebLink">Weblink</param>
	public PdfAnnotation
			(
			PdfPage				Page,
			PdfRectangle		AnnotRect,
			PdfWebLink			WebLink
			) : base(Page.Document)
		{
		// annotation subtype
		Dictionary.Add("/Subtype", "/Link");

		// action reference dictionary
		Dictionary.AddIndirectReference("/A", WebLink);

		// constructor helper method
		ContructorHelper(Page, AnnotRect);

		// exit
		return;
		}

	/// <summary>
	/// PDF screen annotation constructor
	/// </summary>
	/// <param name="Page">Associated page</param>
	/// <param name="AnnotRect">Annotation rectangle</param>
	/// <param name="DisplayMedia">Display media class</param>
	public PdfAnnotation
			(
			PdfPage				Page,
			PdfRectangle		AnnotRect,
			PdfDisplayMedia		DisplayMedia
			) : base(Page.Document)
		{
		// annotation subtype
		Dictionary.Add("/Subtype", "/Screen");

		// action reference dictionary
		Dictionary.AddIndirectReference("/A", DisplayMedia);
		this.DisplayMedia = DisplayMedia;

		// add page reference
		Dictionary.AddIndirectReference("/P", Page);

		// add annotation reference
		DisplayMedia.Dictionary.AddIndirectReference("/AN", this);

		// constructor helper method
		ContructorHelper(Page, AnnotRect);

		// exit
		return;
		}

	/// <summary>
	/// File attachement
	/// </summary>
	/// <param name="Page">Associated page</param>
	/// <param name="AnnotRect">Annotation rectangle</param>
	/// <param name="EmbeddedFile">Embedded file name</param>
	/// <param name="Icon">Icon</param>
	/// <remarks>
	/// <para>
	/// PDF specifications File Attachment Annotation page 637 table 8.35
	/// </para>
	/// </remarks>
	public PdfAnnotation
			(
			PdfPage				Page,
			PdfRectangle		AnnotRect,
			PdfEmbeddedFile		EmbeddedFile,
			FileAttachIcon		Icon = FileAttachIcon.PushPin
			) : base(Page.Document)
		{
		// annotation subtype
		Dictionary.Add("/Subtype", "/FileAttachment");

		// add page reference
		Dictionary.AddIndirectReference("/FS", EmbeddedFile);

		// icon
		Dictionary.AddName("/Name", Icon.ToString());

		// constructor helper method
		ContructorHelper(Page, AnnotRect);

		// exit
		return;
		}

	private void ContructorHelper
			(
			PdfPage				Page,
			PdfRectangle		AnnotRect
			)
		{
		// save rectangle
		this.AnnotRect = AnnotRect;

		// area rectangle on the page
		Dictionary.AddRectangle("/Rect", AnnotRect);

		// annotation flags. value of 4 = Bit position 3 print
		Dictionary.Add("/F", "4");

		// add annotation object to page dictionary
		PdfKeyValue KeyValue = Page.Dictionary.GetValue("/Annots");
		if(KeyValue == null)
			{
			Page.Dictionary.AddFormat("/Annots", "[{0} 0 R]", ObjectNumber);
			}

		else
			{
			Page.Dictionary.Add("/Annots", ((String) KeyValue.Value).Replace("]", String.Format(" {0} 0 R]", ObjectNumber)));
			}

		// border style dictionary. If /BS with /W 0 is not specified, the annotation will have a thin border
		Dictionary.Add("/BS", "<</W 0>>");

		// exit
		return;
		}

	/// <summary>
	/// Gets a copy of the annotation rectangle
	/// </summary>
	public PdfRectangle AnnotationRect
		{
		get
			{
			return(new PdfRectangle(AnnotRect));
			}
		}

	/// <summary>
	/// Activate annotation when page becomes visible.
	/// </summary>
	/// <param name="Activate">Activate or not-activate annotation.</param>
	public void ActivateActionWhenPageIsVisible
			(
			Boolean	Activate
			)
		{
		// applicable to screen action
		if(DisplayMedia != null)
			{
			// play video when page becomes visible
			if(Activate) Dictionary.AddFormat("/AA", "<</PV {0} 0 R>>", DisplayMedia.ObjectNumber);
			else Dictionary.Remove("/AA");
			}
		return;
		}

	/// <summary>
	/// Display border around annotation rectangle.
	/// </summary>
	/// <param name="BorderWidth">Border width</param>
	public void DisplayBorder
			(
			Double	BorderWidth
			)
		{
		// see page 611 section 8.4
		Dictionary.AddFormat("/BS", "<</W {0}>>", ToPt(BorderWidth));
		return;
		}

	/// <summary>
	/// Annotation rectangle appearance
	/// </summary>
	/// <param name="AppearanceDixtionary">PDF X Object</param>
	public void Appearance
			(
			PdfXObject	AppearanceDixtionary
			)
		{
		Dictionary.AddFormat("/AP", "<</N {0} 0 R>>", AppearanceDixtionary.ObjectNumber);
		return;
		}
	}
}
