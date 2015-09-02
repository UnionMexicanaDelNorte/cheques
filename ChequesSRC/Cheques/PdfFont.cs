/////////////////////////////////////////////////////////////////////
//
//	PdfFileWriter
//	PDF File Write C# Class Library.
//
//	PdfFont
//	PDF Font resource.
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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;

namespace PdfFileWriter
{
/////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF font descriptor flags enumeration
/// </summary>
/////////////////////////////////////////////////////////////////////
public enum PdfFontFlags
	{
	/// <summary>
	/// None
	/// </summary>
	None = 0,

	/// <summary>
	/// Fixed pitch font
	/// </summary>
	FixedPitch = 1,

	/// <summary>
	/// Serif font
	/// </summary>
	Serif = 1 << 1,

	/// <summary>
	/// Symbolic font
	/// </summary>
	Symbolic = 1 << 2,

	/// <summary>
	/// Script font
	/// </summary>
	Script = 1 << 3,

	/// <summary>
	/// Non-symbolic font
	/// </summary>
	Nonsymbolic = 1 << 5,

	/// <summary>
	/// Italic font
	/// </summary>
	Italic = 1 << 6,

	/// <summary>
	/// All cap font
	/// </summary>
	AllCap = 1 << 16,

	/// <summary>
	/// Small cap font
	/// </summary>
	SmallCap = 1 << 17,

	/// <summary>
	/// Force bold font
	/// </summary>
	ForceBold = 1 << 18
	}

/////////////////////////////////////////////////////////////////////
/// <summary>
/// Kerning adjustment class
/// </summary>
/// <remarks>
/// Text position adjustment for TJ operator.
/// The adjustment is for a font height of one point.
/// Mainly used for font kerning.
/// </remarks>
/////////////////////////////////////////////////////////////////////
public class KerningAdjust
	{
	/// <summary>
	/// Gets or sets Text
	/// </summary>
	public String	Text {get; set;}

	/// <summary>
	/// Gets or sets adjustment
	/// </summary>
	public Double	Adjust {get; set;}

	/// <summary>
	/// Kerning adjustment constructor
	/// </summary>
	/// <param name="Text">Text</param>
	/// <param name="Adjust">Adjustment</param>
	public KerningAdjust
			(
			String	Text,
			Double	Adjust
			)
		{
		this.Text = Text;
		this.Adjust = Adjust;
		return;
		}
	}

// Original character code to substitute code
internal class OrigToSub : IComparable<OrigToSub>
	{
	// Original character code
	internal Int32	Orig {get; set;}

	// Substitute character code
	internal Int32	Sub {get; set;}

	internal OrigToSub(Int32 Orig)
		{
		this.Orig = Orig;
		return;
		}

	internal OrigToSub(Int32 Orig, Int32 Sub)
		{
		this.Orig = Orig;
		this.Sub = Sub;
		return;
		}

	public Int32 CompareTo(OrigToSub Other)
		{
		return(this.Orig - Other.Orig);
		}
	}

////////////////////////////////////////////////////////////////////
/// <summary>
/// PDF font class
/// </summary>
/// <remarks>
/// <para>
/// For more information go to <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#LanguageSupport">2.3 Language Support</a>
/// </para>
/// <para>
/// <a href="http://www.codeproject.com/Articles/570682/PDF-File-Writer-Csharp-Class-Library-Version#FontResources">For example of defining font resources see 3.2. Font Resources</a>
/// </para>
/// </remarks>
////////////////////////////////////////////////////////////////////
public class PdfFont : PdfObject
	{
	internal	FontApi			FontInfo;
	internal	Boolean[]		ActiveChar;			// 0 to 255
	internal	Int32[]			CharSubArray;		// 0 to 255
	internal	List<OrigToSub>	OrigToSubArray;
	internal	Boolean			SymbolicFont;

	private		FontFamily		FontFamily;
	private		Boolean			EmbeddedFont;
	private		Font			DesignFont;
	private		PdfFontFlags	FontFlags;
	private		Double			PdfLineSpacing;
	private		Double			PdfAscent;
	private		Double			PdfDescent;
	private		Double			PdfLeading {get{return(PdfLineSpacing - PdfAscent - PdfDescent);}}
	private		Double			PdfCapHeight;
	private		Double			PdfStrikeoutWidth;
	private		Double			PdfStrikeoutPosition;
	private		Double			PdfUnderlineWidth;
	private		Double			PdfUnderlinePosition;
	private		Double			PdfSubscriptSize;
	private		Double			PdfSubscriptPosition;
	private		Double			PdfSuperscriptSize;
	private		Double			PdfSuperscriptPosition;
	private		Double			PdfItalicAngle;
	private		Int32			PdfFontWeight;
	private		Int32			DesignHeight;
	private		PdfObject		FontDescriptor;
	private		PdfObject		FontWidthArray;
	private		Double[]		CharWidthArray;		// 0 to 255
	private		BoundingBox[]	GlyphArray;

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// PDF Font resource constructor
	/// </summary>
	/// <param name="Document">Document object</param>
	/// <param name="FontFamilyName">Font family name</param>
	/// <param name="FontStyle">Font style</param>
	/// <param name="EmbeddedFont">Embedded font</param>
	////////////////////////////////////////////////////////////////////
	public PdfFont
			(
			PdfDocument		Document,			// PDF document main object
			String			FontFamilyName,		// font family name
			FontStyle		FontStyle,			// font style (Regular, Bold, Italic or Bold | Italic
			Boolean			EmbeddedFont = true	// embed font in PDF document file
			) : base(Document, ObjectType.Dictionary, "/Font")
		{
		// save embedded font flag
		this.EmbeddedFont = EmbeddedFont;
 
		// font style cannot be underline or strikeout
		if((FontStyle & (FontStyle.Underline | FontStyle.Strikeout)) != 0)
			throw new ApplicationException("Font resource cannot have underline or strikeout");

		// create resource code
		ResourceCode = Document.GenerateResourceNumber('F');

		// get font family structure
		FontFamily = new FontFamily(FontFamilyName);

		// test font style availability
		if(!FontFamily.IsStyleAvailable(FontStyle)) throw new ApplicationException("Font style not available for font family");

		// design height
		DesignHeight = FontFamily.GetEmHeight(FontStyle);

		// Ascent, descent and line spacing for a one point font
		PdfAscent = WindowsToPdf(FontFamily.GetCellAscent(FontStyle));
		PdfDescent = WindowsToPdf(FontFamily.GetCellDescent(FontStyle)); // positive number
		PdfLineSpacing = WindowsToPdf(FontFamily.GetLineSpacing(FontStyle));

		// create design font
		DesignFont = new Font(FontFamily, DesignHeight, FontStyle, GraphicsUnit.Pixel);

		// create windows sdk font info object
		FontInfo = new FontApi(DesignFont, DesignHeight);

		// get character width array
		CharWidthArray = FontInfo.GetCharWidthApi(0, 255);

		// get array of glyph bounding box and width
		GlyphArray = FontInfo.GetGlyphMetricsApi(0, 255);

		// reset active (used) characters to not used
		ActiveChar = new Boolean[256];

		// get outline text metrics structure
		WinOutlineTextMetric OTM = FontInfo.GetOutlineTextMetricsApi();

		// license
		if((OTM.otmfsType & 2) != 0) throw new ApplicationException("Font " + FontFamilyName + " is not licensed for embedding");

		// make sure we have true type font and not device font
		if((OTM.otmTextMetric.tmPitchAndFamily & 0xe) != 6) throw new ApplicationException("Font must be True Type and vector"); 

		// PDF font flags
		FontFlags = 0;
		if((OTM.otmfsSelection & 1) != 0) FontFlags |= PdfFontFlags.Italic;
		// roman font is a serif font
		if((OTM.otmTextMetric.tmPitchAndFamily >> 4) == 1) FontFlags |= PdfFontFlags.Serif;
		if((OTM.otmTextMetric.tmPitchAndFamily >> 4) == 4) FontFlags |= PdfFontFlags.Script;
		// #define SYMBOL_CHARSET 2
		if(OTM.otmTextMetric.tmCharSet == 2)
			{
			FontFlags |= PdfFontFlags.Symbolic;
			SymbolicFont = true;
			}
		else
			{
			FontFlags |= PdfFontFlags.Nonsymbolic;
			SymbolicFont = false;
			}

		// #define TMPF_FIXED_PITCH 0x01 (Note very carefully that those meanings are the opposite of what the constant name implies.)
		if((OTM.otmTextMetric.tmPitchAndFamily & 1) == 0) FontFlags |= PdfFontFlags.FixedPitch;

		// strikeout
		PdfStrikeoutPosition = WindowsToPdf(OTM.otmsStrikeoutPosition);
		PdfStrikeoutWidth = WindowsToPdf((Int32) OTM.otmsStrikeoutSize);

		// underline
		PdfUnderlinePosition = WindowsToPdf(OTM.otmsUnderscorePosition);
		PdfUnderlineWidth = WindowsToPdf(OTM.otmsUnderscoreSize);

		// subscript
		PdfSubscriptSize = WindowsToPdf(OTM.otmptSubscriptSize.Y);
		PdfSubscriptPosition = WindowsToPdf(OTM.otmptSubscriptOffset.Y);

		// superscript
		PdfSuperscriptSize = WindowsToPdf(OTM.otmptSuperscriptSize.Y);
		PdfSuperscriptPosition = WindowsToPdf(OTM.otmptSuperscriptOffset.Y);

		PdfItalicAngle = (Double) OTM.otmItalicAngle / 10.0;
		PdfFontWeight = OTM.otmTextMetric.tmWeight;

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Validate character
	/// </summary>
	/// <param name="TestChar">Character</param>
	/// <returns>Returns the character or its substitute.</returns>
	/// <remarks>
	/// This program accepts character codes 32 to 126 and 160 to 255 and
	/// Unicode characters that were substituted.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public Int32 ValidateChar
			(
			Char	TestChar
			)
		{
		// character is one byte long
		if(TestChar <= 255)
			{
			// test for control characters
			if(TestChar < ' ' || TestChar > '~' && TestChar < 160) throw new ApplicationException("No support for control characters");

			// return the same character
			return(TestChar);
			}

		// test for substitute array
		if(OrigToSubArray != null)
			{
			// search substitute array
			Int32 index = OrigToSubArray.BinarySearch(new OrigToSub(TestChar));

			// if found, return the substitute character
			if(index >= 0) return(OrigToSubArray[index].Sub);
			}

		// there is no substitute array or input character is not in the substitute array
		throw new ApplicationException("No support for characters greater than 255 without substitution");
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Character substitution
	/// </summary>
	/// <param name="OrigFrom">Start of actual Unicode range.</param>
	/// <param name="OrigTo">End of actual Unicode range.</param>
	/// <param name="DestFrom">Start of substitute range less than 256.</param>
	/// <remarks>
	/// This program accepts character codes 32 to 126 and 160 to 255.
	/// If the font supports characters above 255, you can map
	/// these codes into the valid region.
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public void CharSubstitution
			(
			Int32	OrigFrom,
			Int32	OrigTo,
			Int32	DestFrom
			)
		{
		// font must be embedded
		if(!EmbeddedFont) throw new ApplicationException("Character substitution is allowed only for embedded fonts");

		// create new character substitution array
		if(CharSubArray == null)
			{
			// create new substitute to original array
			CharSubArray = new Int32[256];
			for(Int32 CharCode = 0; CharCode < 256; CharCode++) CharSubArray[CharCode] = CharCode;

			// create new original character to substitute
			OrigToSubArray = new List<OrigToSub>();
			}

		// character count
		Int32 Count = OrigTo - OrigFrom + 1;

		// get character width for origin array
		Double[] SubWidthArray = FontInfo.GetCharWidthApi(OrigFrom, OrigTo);

		// get array of glyph bounding box and width for origin array
		BoundingBox[] SubGlyphArray = FontInfo.GetGlyphMetricsApi(OrigFrom, OrigTo);

		// replace existing character width and bounding box
		for(Int32 Ptr = 0; Ptr < Count; Ptr++)
			{
			// destination must be 33 to 126 or 161 to 255
			// note: space and non breaking space cannot be used for substitution
			Int32 DestChar = DestFrom + Ptr;
			if(DestChar <= ' ' || DestChar > '~' && DestChar <= 160 || DestChar > 255)
				throw new ApplicationException("Invalid character substitution. Destination must be 33 to 126 or 161 to 255.");

			// search origin to substitute array
			Int32 OrigChar = OrigFrom + Ptr;
			OrigToSub origToSub = new OrigToSub(OrigChar, DestChar);
			Int32 index = OrigToSubArray.BinarySearch(origToSub);

			// duplicate origin overlaping substitution ranges
			if(index >= 0) throw new ApplicationException("CharSubstitution duplicate origin character");

			// test for duplicate destination
			foreach(OrigToSub OTS in OrigToSubArray) if(OTS.Sub == DestChar) throw new ApplicationException("CharSubstitution duplicate destibation character");

			// add a new record
			OrigToSubArray.Insert(~index, origToSub);

			// save origin char in destination array
			CharSubArray[DestChar] = OrigChar;

			// change character width and glyph bounding box to origin value
			CharWidthArray[DestChar] = SubWidthArray[Ptr];
			GlyphArray[DestChar] = SubGlyphArray[Ptr];
			}

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Convert c# text string to PDF text format
	/// </summary>
	/// <param name="Text">Source c# text</param>
	/// <returns>Destination PDF text</returns>
	////////////////////////////////////////////////////////////////////
	public String PdfText
			(
			String		Text
			)
		{
		Boolean Printable = true;

		// copy text to temp string builder
		StringBuilder TempText = new StringBuilder(Text);

		// scan string
		for(Int32 index = 0; index < Text.Length; index++)
			{
			// validate character and replace characters that were substituted
			Int32 NewChar = ValidateChar(TempText[index]);
			TempText[index] = (Char) NewChar;

			// indicate character was used for embeded fond process
			ActiveChar[NewChar] = true;

			// set flag to inmdicate output form () or <>
			if(NewChar > '~') Printable = false;
			}

		// text is all printable characters (' ' to '~')
		if(Printable)
			{
			// test for ( and ) and \
			// add backslash
			TempText.Replace("\\", "\\\\");
			TempText.Replace("(", "\\(");
			TempText.Replace(")", "\\)");

			// add ()
			TempText.Insert(0, "(");
			TempText.Append(")");
			return(TempText.ToString());
			}

		// convert to hex string if at least one character is over 0x80
		StringBuilder HexText = new StringBuilder("<");
		for(Int32 index = 0; index < Text.Length; index++)
			{
			HexText.AppendFormat("{0:x2}", (Int32) TempText[index]);
			}
		HexText.Append(">");
		return(HexText.ToString());
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Windows design units to PDF design units
	/// </summary>
	/// <param name="Value">Font design value</param>
	/// <returns>Font PDF value</returns>
	/// <remarks>
	/// Font size is set to 1 point
	/// </remarks>
	////////////////////////////////////////////////////////////////////
	public Double WindowsToPdf
			(
			Int32	Value
			)
		{
		return((Double) Value / (Double) DesignHeight);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font units to user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="Value">Design value</param>
	/// <returns>Design value in user units</returns>
	////////////////////////////////////////////////////////////////////
	public Double FontUnitsToUserUnits
			(
			Double		FontSize,
			Double		Value
			)
		{
		return(Value * FontSize / ScaleFactor);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font units to PDF dictionary values
	/// </summary>
	/// <param name="Value">Font design value</param>
	/// <returns>PDF dictionary value</returns>
	////////////////////////////////////////////////////////////////////
	public Double FontUnitsToPdfDic
			(
			Double		Value
			)
		{
		return(Round(1000.0 * Value));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Line spacing in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Line spacing</returns>
	////////////////////////////////////////////////////////////////////
	public Double LineSpacing
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfLineSpacing));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font ascent in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Font ascent</returns>
	////////////////////////////////////////////////////////////////////
	public Double Ascent
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfAscent));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font ascent in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Font ascent plus half of internal leading.</returns>
	////////////////////////////////////////////////////////////////////
	public Double AscentPlusLeading
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfAscent + PdfLeading / 2));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font descent in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Font descent</returns>
	////////////////////////////////////////////////////////////////////
	public Double Descent
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfDescent));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Font descent in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Font descent plus half of internal leading.</returns>
	////////////////////////////////////////////////////////////////////
	public Double DescentPlusLeading
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfDescent + PdfLeading / 2));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Capital M height in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Capital M height</returns>
	////////////////////////////////////////////////////////////////////
	public Double CapHeight
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfCapHeight));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Strikeout position in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Strikeout position</returns>
	////////////////////////////////////////////////////////////////////
	public Double StrikeoutPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfStrikeoutPosition));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Strikeout width in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Strikeout line width.</returns>
	////////////////////////////////////////////////////////////////////
	public Double StrikeoutWidth
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfStrikeoutWidth));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Underline position in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Underline position</returns>
	////////////////////////////////////////////////////////////////////
	public Double UnderlinePosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfUnderlinePosition));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Underline width in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Underline line width.</returns>
	////////////////////////////////////////////////////////////////////
	public Double UnderlineWidth
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfUnderlineWidth));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Subscript position in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Subscript position</returns>
	////////////////////////////////////////////////////////////////////
	public Double SubscriptPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfSubscriptPosition));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Subscript character size in points
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Subscript font size</returns>
	////////////////////////////////////////////////////////////////////
	public Double SubscriptSize
			(
			Double		FontSize
			)
		{
		return(FontSize * PdfSubscriptSize);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Superscript character position
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Superscript position</returns>
	////////////////////////////////////////////////////////////////////
	public Double SuperscriptPosition
			(
			Double		FontSize
			)
		{
		return(FontUnitsToUserUnits(FontSize, PdfSuperscriptPosition));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Superscript character size in points
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <returns>Superscript font size</returns>
	////////////////////////////////////////////////////////////////////
	public Double SuperscriptSize
			(
			Double		FontSize
			)
		{
		return(FontSize * PdfSuperscriptSize);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Character width in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="CharValue">Character code</param>
	/// <returns>Character width</returns>
	////////////////////////////////////////////////////////////////////
	public Double CharWidth
			(
			Double	FontSize,
			Char	CharValue
			)
		{
		return(FontUnitsToUserUnits(FontSize, CharWidthArray[ValidateChar(CharValue)]));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Character width in user units
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="DrawStyle">Draw style</param>
	/// <param name="CharValue">Character code</param>
	/// <returns>Character width</returns>
	////////////////////////////////////////////////////////////////////
	public Double CharWidth
			(
			Double		FontSize,
			DrawStyle	DrawStyle,
			Char		CharValue
			)
		{
		Int32 SubChar = ValidateChar(CharValue);

		if((DrawStyle & (DrawStyle.Subscript | DrawStyle.Superscript)) == 0) return(FontUnitsToUserUnits(FontSize, CharWidthArray[SubChar]));

		if((DrawStyle & DrawStyle.Superscript) != 0) return(FontUnitsToUserUnits(SubscriptSize(FontSize), CharWidthArray[SubChar]));

		return(FontUnitsToUserUnits(SuperscriptSize(FontSize), CharWidthArray[SubChar]));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Text width
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="Text">Text</param>
	/// <returns>Width</returns>
	////////////////////////////////////////////////////////////////////
	public Double TextWidth
			(
			Double	FontSize,
			String	Text
			)
		{
		Double Width = 0;
		for(Int32 Index = 0; Index < Text.Length; Index++)
			{
			// character width
			Width += CharWidthArray[ValidateChar(Text[Index])];
			}

		// to user unit of measure
		return(FontUnitsToUserUnits(FontSize, Width));
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Word spacing to stretch text to given width
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="ReqWidth">Required width</param>
	/// <param name="WordSpacing">Output word spacing</param>
	/// <param name="CharSpacing">Output character spacing</param>
	/// <param name="Text">Text</param>
	/// <returns>True-done, False-not done.</returns>
	////////////////////////////////////////////////////////////////////
	public Boolean TextFitToWidth
			(
			Double		FontSize,
			Double		ReqWidth,
			out Double	WordSpacing,
			out Double	CharSpacing,
			String		Text
			)
		{
		WordSpacing = 0;
		CharSpacing = 0;
		if(Text == null || Text.Length < 2) return(false);

		Double Width = 0;
		Int32 SpaceCount = 0;
		foreach(Char Chr in Text)
			{
			// validate
			Int32 SubChr = ValidateChar(Chr);

			// character width
			Width += CharWidthArray[SubChr];

			// space count
			if(SubChr == ' ') SpaceCount++;
			}

		// to user unit of measure
		Width = FontUnitsToUserUnits(FontSize, Width);

		// extra spacing required
		Double ExtraSpace = ReqWidth - Width;

		// highest possible resolution (12000 dots per inch)
		Double MaxRes = 0.006 / ScaleFactor;

		// string is too wide
		if(ExtraSpace < (-MaxRes)) return(false);

		// string is just right
		if(ExtraSpace < MaxRes) return(true);

		// String does not have any space
		if(SpaceCount == 0)
			{
			CharSpacing = ExtraSpace / (Text.Length - 1);
			return(true);
			}

		// extra space per word
		WordSpacing = ExtraSpace / SpaceCount;

		// extra space is equal or less than one blank
		if(WordSpacing <= FontUnitsToUserUnits(FontSize, CharWidthArray[(Byte) ' '])) return(true);

		// extra space is larger that one blank
		// increase character and word spacing
		CharSpacing = ExtraSpace / (10 * SpaceCount + Text.Length - 1);
		WordSpacing = 10 * CharSpacing;
		return(true);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Text bounding box in user coordinate units.
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="Text">Text</param>
	/// <returns>Bounding box</returns>
	////////////////////////////////////////////////////////////////////
	public BoundingBox TextBoundingBox
			(
			Double	FontSize,
			String	Text
			)
		{
		if(String.IsNullOrEmpty(Text)) return(null);

		// initialize result box to first character
		BoundingBox Box = new BoundingBox(GlyphArray[ValidateChar(Text[0])]);

		// loop from second character
		for(Int32 Index = 1; Index < Text.Length; Index++)
			{
			// get bounding box for current character
			BoundingBox TempBox = GlyphArray[ValidateChar(Text[Index])];

			// update bottom
			if(TempBox.Rect.Bottom < Box.Rect.Bottom) Box.Rect.Bottom = TempBox.Rect.Bottom;

			// update top
			if(TempBox.Rect.Top > Box.Rect.Top) Box.Rect.Top = TempBox.Rect.Top;

			// update overall advanced width
			Box.Width += TempBox.Width;
			}

		// update right side
		if(Text.Length > 1)
			{
			// last character
			BoundingBox TempBox = GlyphArray[ValidateChar(Text[Text.Length - 1])];
			Box.Rect.Right = Box.Width - TempBox.Width + TempBox.Rect.Right;
			}

		// convert to user coordinate units
		Double Factor = FontSize / ScaleFactor;
		Box.Rect.Left = Factor * Box.Rect.Left;
		Box.Rect.Bottom = Factor * Box.Rect.Bottom;
		Box.Rect.Right = Factor * Box.Rect.Right;
		Box.Rect.Top = Factor * Box.Rect.Top;
		Box.Width = Factor * Box.Width;
		return(Box);
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Text Kerning
	/// </summary>
	/// <param name="Text">Text</param>
	/// <returns>Kerning adjustment pairs</returns>
	////////////////////////////////////////////////////////////////////
	public KerningAdjust[] TextKerning
			(
			String	Text
			)
		{
		// string is empty or one character
		if(String.IsNullOrEmpty(Text) || Text.Length == 1) return(null);

		// find first and last characters of the text
		Int32 First = ValidateChar(Text[0]);
		Int32 Last = ValidateChar(Text[0]);
		foreach(Char Chr in Text)
			{
			Int32 SubChr = ValidateChar(Chr);
			if(SubChr < First) First = SubChr;
			else if(SubChr > Last) Last = SubChr;
			}

		// get kerning information
		WinKerningPair[] KP = FontInfo.GetKerningPairsApi(First, Last);

		// no kerning info available for this font or for this range
		if(KP == null) return(null);

		// prepare a list of kerning adjustments
		List<KerningAdjust> KA = new List<KerningAdjust>();

		// look for pairs with adjustments
		Int32 Ptr1 = 0;
		for(Int32 Ptr2 = 1; Ptr2 < Text.Length; Ptr2++)
			{
			// search for a pair of characters
			Int32 Index = Array.BinarySearch(KP, new WinKerningPair((Char) ValidateChar(Text[Ptr2 - 1]), (Char) ValidateChar(Text[Ptr2])));

			// not kerning information for this pair
			if(Index < 0) continue;

			// add kerning adjustment in PDF font units (windows design units divided by windows font design height)
			KA.Add(new KerningAdjust(Text.Substring(Ptr1, Ptr2 - Ptr1), KP[Index].KernAmount));

			// adjust pointer
			Ptr1 = Ptr2;
			}

		// list is empty
		if(KA.Count == 0) return(null);

		// add last
		KA.Add(new KerningAdjust(Text.Substring(Ptr1, Text.Length - Ptr1), 0));

		// exit
		return(KA.ToArray());
		}

	////////////////////////////////////////////////////////////////////
	/// <summary>
	/// Text kerning width
	/// </summary>
	/// <param name="FontSize">Font size</param>
	/// <param name="KerningArray">Kerning array</param>
	/// <returns>Width</returns>
	////////////////////////////////////////////////////////////////////
	public Double TextKerningWidth
			(
			Double			FontSize,		// in points
			KerningAdjust[]	KerningArray
			)
		{
		// text is null or empty
		if(KerningArray == null || KerningArray.Length == 0) return(0);

		// total width
		Double Width = 0;

		// draw text
		Int32 LastStr = KerningArray.Length - 1;
		for(Int32 Index = 0; Index < LastStr; Index++)
			{
			KerningAdjust KA = KerningArray[Index];
			Width += TextWidth(FontSize, KA.Text) + FontUnitsToUserUnits(FontSize, KA.Adjust);
			}

		// last string
		Width += TextWidth(FontSize, KerningArray[LastStr].Text);
		return(Width);
		}

	////////////////////////////////////////////////////////////////////
	// Write object to PDF file
	////////////////////////////////////////////////////////////////////

	internal override void WriteObjectToPdfFile()
		{
		// look for first and last character
		Int32 FirstChar;
		Int32 LastChar;
		for(FirstChar = 0; FirstChar < 256 && !ActiveChar[FirstChar]; FirstChar++);
		if(FirstChar == 256) return;
		for(LastChar = 255; !ActiveChar[LastChar]; LastChar--);

		// pdf font name
		StringBuilder PdfFontName = new StringBuilder("/");

		// for embedded font add 6 alpha characters prefix
		if(EmbeddedFont)
			{
			PdfFontName.Append("PFWAAA+");
			Int32 Ptr1 = 6;
			for(Int32 Ptr2 = ResourceCode.Length - 1; Ptr2 >= 0 && Char.IsDigit(ResourceCode[Ptr2]); Ptr2--)
				{
				PdfFontName[Ptr1--] =  (Char) ((Int32) ResourceCode[Ptr2] + ('A' - '0'));
				}
			}

		// PDF readers are not happy with space in font name
		PdfFontName.Append(FontFamily.Name.Replace(" ", "#20"));

		// font name
		if((DesignFont.Style & FontStyle.Bold) != 0)
			{
			if((DesignFont.Style & FontStyle.Italic) != 0)
				PdfFontName.Append(",BoldItalic");
			else
				PdfFontName.Append(",Bold");
			}
		else if((DesignFont.Style & FontStyle.Italic) != 0)
			{
			PdfFontName.Append(",Italic");
			}

		// add items to dictionary
		Dictionary.Add("/Subtype", "/TrueType");
		Dictionary.Add("/BaseFont", PdfFontName.ToString());

		// add first and last characters
		Dictionary.AddInteger("/FirstChar", FirstChar);
		Dictionary.AddInteger("/LastChar", LastChar);

		// create font descriptor
		FontDescriptor = new PdfObject(Document, ObjectType.Dictionary, "/FontDescriptor");

		// add link to font object
		Dictionary.AddIndirectReference("/FontDescriptor", FontDescriptor);

		// font descriptor dictionary
		FontDescriptor.Dictionary.Add("/FontName", PdfFontName.ToString());	// must be the same as BaseFont above
		FontDescriptor.Dictionary.AddInteger("/Flags", (Int32) FontFlags);
		FontDescriptor.Dictionary.AddReal("/ItalicAngle", PdfItalicAngle);
		FontDescriptor.Dictionary.AddInteger("/FontWeight", PdfFontWeight);
		FontDescriptor.Dictionary.AddReal("/Leading", FontUnitsToPdfDic(PdfLeading));
		FontDescriptor.Dictionary.AddReal("/Ascent", FontUnitsToPdfDic(PdfAscent));
		FontDescriptor.Dictionary.AddReal("/Descent", FontUnitsToPdfDic(-PdfDescent));

		// alphabetic (non symbolic) fonts
		if((FontFlags & PdfFontFlags.Symbolic) == 0)
			{
			Dictionary.Add("/Encoding", "/WinAnsiEncoding");
			BoundingBox Box = FontInfo.GetGlyphMetricsApi('x');
			FontDescriptor.Dictionary.AddReal("/XHeight", FontUnitsToPdfDic(Box.Rect.Top));
			FontDescriptor.Dictionary.AddReal("/AvgWidth", FontUnitsToPdfDic(Box.Width));
			Box = FontInfo.GetGlyphMetricsApi('M');
			PdfCapHeight = Box.Rect.Top;
			FontDescriptor.Dictionary.AddReal("/CapHeight", FontUnitsToPdfDic(Box.Rect.Top));
			FontDescriptor.Dictionary.AddReal("/StemV", StemV());
			}

		// create width object array
		FontWidthArray = new PdfObject(Document, ObjectType.Other);

		// add link to font object
		Dictionary.AddIndirectReference("/Widths", FontWidthArray);

		// build bounding box and width array
		Double Left = Double.MaxValue;
		Double Bottom = Double.MaxValue;
		Double Right = Double.MinValue;
		Double Top = Double.MinValue;
		Double MaxWidth = Double.MinValue;
		FontWidthArray.ContentsString.Append("[");

		Int32 EolLength = 100;
		for(Int32 Index = FirstChar; Index <= LastChar; Index++)
			{
			Double CharWidth;

			// not used
			if(!ActiveChar[Index])
				{
				CharWidth = 0;
				}

			// used
			else
				{
				// bounding box
				BoundingBox GM = GlyphArray[Index];
				if(GM.Rect.Left < Left) Left = GM.Rect.Left;
				if(GM.Rect.Bottom < Bottom) Bottom = GM.Rect.Bottom;
				if(GM.Rect.Right > Right) Right = GM.Rect.Right;
				if(GM.Rect.Top > Top) Top = GM.Rect.Top;

				// character advance width
				CharWidth = GM.Width;

				// max width
				if(CharWidth > MaxWidth) MaxWidth = CharWidth;
				}

			// add width to width array
			if(FontWidthArray.ContentsString.Length > EolLength)
				{
				FontWidthArray.ContentsString.Append('\n');
				EolLength = FontWidthArray.ContentsString.Length + 100;
				}

			// add width to width array
			FontWidthArray.ContentsString.AppendFormat(NFI.PeriodDecSep, "{0} ", FontUnitsToPdfDic(CharWidth));
			}

		// add to font descriptor array
		FontDescriptor.Dictionary.AddReal("/MaxWidth", FontUnitsToPdfDic(MaxWidth));
		FontDescriptor.Dictionary.AddFormat("/FontBBox", "[{0} {1} {2} {3}]",
			FontUnitsToPdfDic(Left), FontUnitsToPdfDic(Bottom), FontUnitsToPdfDic(Right), FontUnitsToPdfDic(Top));

		// terminate width array
		FontWidthArray.ContentsString.Length--;
		FontWidthArray.ContentsString.Append("]");

		// create font file
		if(EmbeddedFont)
			{
			// create font file stream
			PdfFontFile EmbeddedFontObj = new PdfFontFile(this);

			// add link to font object
			FontDescriptor.Dictionary.AddIndirectReference("/FontFile2", EmbeddedFontObj);
			}

		// call base write PdfObject to file method
		base.WriteObjectToPdfFile();

		// exit
		return;
		}

	////////////////////////////////////////////////////////////////////
	// Calculate StemV from capital I
	////////////////////////////////////////////////////////////////////

	private Double StemV()
		{
		// convert I to graphics path
		GraphicsPath GP = new GraphicsPath();
		GP.AddString("I", FontFamily, (Int32) DesignFont.Style, 1000, Point.Empty, StringFormat.GenericDefault);

		// center point of I
		RectangleF Rect = GP.GetBounds();
		Int32 X = (Int32) ((Rect.Left + Rect.Right) / 2);
		Int32 Y = (Int32) ((Rect.Bottom + Rect.Top) / 2);

		// bounding box converted to integer
		Int32 LeftLimit = (Int32) Rect.Left;
		Int32 RightLimit = (Int32) Rect.Right;

		// make sure we are within the I
		if(!GP.IsVisible(X, Y)) return((Double) (RightLimit - LeftLimit));

		// look for left edge
		Int32 Left;
		for(Left = X - 1; Left >= LeftLimit && GP.IsVisible(Left, Y); Left--);

		// look for right edge
		Int32 Right;
		for(Right = X + 1; Right < RightLimit && GP.IsVisible(Right, Y); Right++);

		// exit
		return((Double) (Right - Left - 1));
		}
	}
}
