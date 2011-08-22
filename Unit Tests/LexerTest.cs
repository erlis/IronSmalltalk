/*
 * **************************************************************************
 *
 * Copyright (c) The IronSmalltalk Project. 
 *
 * This source code is subject to terms and conditions of the 
 * license agreement found in the solution directory. 
 * See: $(SolutionDir)\License.htm ... in the root of this distribution.
 * By using this source code in any fashion, you are agreeing 
 * to be bound by the terms of the license agreement.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **************************************************************************
*/

using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using IronSmalltalk.Compiler.LexicalAnalysis;
using IronSmalltalk.Compiler.LexicalTokens;
using System.Numerics;
using IronSmalltalk.Common;

namespace IronSmalltalk.UnitTesting
{
    [TestClass]
    public class LexerTest
    {
        [TestMethod]
        public void Test_3_5_2_Comments_A()
        {
            Scanner lexer = this.GetLexer("\" This is a comment \n with two lines \"");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CommentToken));
            CommentToken token = (CommentToken) obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(37, token.StopPosition.Position);
            Assert.AreEqual(" This is a comment \n with two lines ", token.Value);
        }

        [TestMethod]
        public void Test_3_5_2_Comments_B()
        {
            // Sequential comments 
            Scanner lexer = this.GetLexer("\" This is a comment \"\" and another comment \"");
            // 1st comment
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CommentToken));
            CommentToken token = (CommentToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(20, token.StopPosition.Position);
            Assert.AreEqual(" This is a comment ", token.Value);
            // 2nd comment
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CommentToken));
            token = (CommentToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(21, token.StartPosition.Position);
            Assert.AreEqual(43, token.StopPosition.Position);
            Assert.AreEqual(" and another comment ", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_3_Identifiers_A()
        {
            Scanner lexer = this.GetLexer("isGood1Time");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            IdentifierToken token = (IdentifierToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(10, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_3_Identifiers_B()
        {
            Scanner lexer = this.GetLexer("_private_Identifier");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            IdentifierToken token = (IdentifierToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(18, token.StopPosition.Position);
            Assert.AreEqual("_private_Identifier", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_3_Identifiers_C()
        {
            // Non-ascii letters
            Scanner lexer = this.GetLexer("üæøåMalmö");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            IdentifierToken token = (IdentifierToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(8, token.StopPosition.Position);
            Assert.AreEqual("üæøåMalmö", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_4_Keywords_A()
        {
            Scanner lexer = this.GetLexer("isGood1Time:");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(KeywordToken));
            KeywordToken token = (KeywordToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(11, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time:", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_4_Keywords_B()
        {
            // An unadorned identifier is an identifier which is not immediately preceded by a '#'. 
            // If a ':' followed by an '=' immediately follows an unadorned identifier, 
            // with no intervening white space, then the token is to be parsed as an
            // identifier followed by an assignmentOperator not as an keyword followed by an '='.
            Scanner lexer = this.GetLexer("isGood1Time:=");
            // Identifier token
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            Assert.IsNotInstanceOfType(obj, typeof(KeywordToken));
            IdentifierToken token = (IdentifierToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(10, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time", token.Value);
            // Assignment token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(AssignmentOperatorToken));
            AssignmentOperatorToken assign = (AssignmentOperatorToken)obj;
            Assert.IsTrue(assign.IsValid);
            Assert.IsNull(assign.ScanError);
            Assert.AreEqual(11, assign.StartPosition.Position);
            Assert.AreEqual(12, assign.StopPosition.Position);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_4_Keywords_C()
        {
            // An unadorned identifier is an identifier which is not immediately preceded by a '#'. 
            // If a ':' followed by an '=' immediately follows an unadorned identifier, 
            // with no intervening white space, then the token is to be parsed as an
            // identifier followed by an assignmentOperator not as an keyword followed by an '='.
            Scanner lexer = this.GetLexer("isGood1Time:true");
            // Identifier token
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(KeywordToken));
            KeywordToken token = (KeywordToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(11, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time:", token.Value);
            // Assignment token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            IdentifierToken idf = (IdentifierToken)obj;
            Assert.IsTrue(idf.IsValid);
            Assert.IsNull(idf.ScanError);
            Assert.AreEqual(12, idf.StartPosition.Position);
            Assert.AreEqual(15, idf.StopPosition.Position);
            Assert.AreEqual("true", idf.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_5_Operators_A()
        {
            Scanner lexer = this.GetLexer("!");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(BinarySelectorToken));
            BinarySelectorToken token = (BinarySelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(0, token.StopPosition.Position);
            Assert.AreEqual("!", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_5_Operators_B()
        {
            Scanner lexer = this.GetLexer("~=");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(BinarySelectorToken));
            BinarySelectorToken token = (BinarySelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            Assert.AreEqual("~=", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_5_Operators_C()
        {
            Scanner lexer = this.GetLexer(@"!%&*+,/<=>?@\~|-");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(BinarySelectorToken));
            BinarySelectorToken token = (BinarySelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(15, token.StopPosition.Position);
            Assert.AreEqual(@"!%&*+,/<=>?@\~|-", token.Value);
            Assert.AreEqual(16, token.Value.Length);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_5_Operators_D()
        {
            Scanner lexer = this.GetLexer("^");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ReturnOperatorToken));
            ReturnOperatorToken token = (ReturnOperatorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(0, token.StopPosition.Position);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_5_Operators_E()
        {
            Scanner lexer = this.GetLexer(":=");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(AssignmentOperatorToken));
            AssignmentOperatorToken token = (AssignmentOperatorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_A()
        {
            // Decimal integer
            Scanner lexer = this.GetLexer("12345");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(SmallIntegerToken));
            SmallIntegerToken token = (SmallIntegerToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(4, token.StopPosition.Position);
            Assert.AreEqual(12345, token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_B()
        {
            // Decimal integer
            Scanner lexer = this.GetLexer("123456789012345");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(LargeIntegerToken));
            LargeIntegerToken token = (LargeIntegerToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(14, token.StopPosition.Position);
            Assert.AreEqual(token.Value, BigInteger.Parse("123456789012345"));
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_C()
        {
            // Radix integer
            Scanner lexer = this.GetLexer("14r10");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(SmallIntegerToken));
            SmallIntegerToken token = (SmallIntegerToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.AreEqual(token.Value, 14);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(4, token.StopPosition.Position);            
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_D()
        {
            // The uppercase alphabetic characters represent the digits with values 10 through 35.
            // It is erroneous if a character representing a digit value greater than or equal to or
            // the numeric value of the radixSpecifier is used to form the radixDigits.
            // Radix integer with invalid radix digits
            Scanner lexer = this.GetLexer("14rABCDEF");
            object obj = lexer.GetToken();
            // Can't test this, since base class is generic :-(
            // Assert.IsInstanceOfType(obj, typeof(LargeIntegerToken));
            Token token = (Token)obj;
            Assert.IsFalse(token.IsValid);
            Assert.IsTrue(token.ScanError.StartsWith("Digit too big.")); // Digit 'E' is too big
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(7, token.StopPosition.Position); // 'E' is part of the token, therefore 7

            lexer = this.GetLexer("14rabc");
            obj = lexer.GetToken();
            // Can't test this, since base class is generic :-(
            // Assert.IsInstanceOfType(obj, typeof(LargeIntegerToken));
            token = (Token)obj;
            Assert.IsFalse(token.IsValid);
            Assert.AreEqual("Invalid radix integer. No digits found after radix specifier.", token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(3, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_E()
        {
            // 2 <= radixSpecifier <= 36.
            // Radix integer with invalid radix
            Scanner lexer = this.GetLexer("1r0");
            object obj = lexer.GetToken();
            // Can't test this, since base class is generic :-(
            // Assert.IsInstanceOfType(obj, typeof(LargeIntegerToken));
            Token token = (Token)obj;
            Assert.IsFalse(token.IsValid);
            Assert.IsTrue(token.ScanError.StartsWith("Invalid radix"));
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);

            lexer = this.GetLexer("37r0");
            obj = lexer.GetToken();
            // Can't test this, since base class is generic :-(
            // Assert.IsInstanceOfType(obj, typeof(LargeIntegerToken));
            token = (Token)obj;
            Assert.IsFalse(token.IsValid);
            Assert.IsTrue(token.ScanError.StartsWith("Invalid radix"));
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(2, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_F()
        {
            // Float
            Scanner lexer = this.GetLexer("123.45");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatDToken)); // FloatD is actually more precise to represent the float than FloatE
            FloatDToken tokenD = (FloatDToken)obj;
            Assert.IsTrue(tokenD.IsValid);
            Assert.IsNull(tokenD.ScanError);
            Assert.AreEqual(0, tokenD.StartPosition.Position);
            Assert.AreEqual(5, tokenD.StopPosition.Position);
            Assert.AreEqual(123.45, tokenD.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("123.0e3");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatEToken));
            FloatEToken token = (FloatEToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(6, token.StopPosition.Position);
            Assert.AreEqual(123000.0, token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("123.0q3");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatDToken));
            tokenD = (FloatDToken)obj;
            Assert.IsTrue(tokenD.IsValid);
            Assert.IsNull(tokenD.ScanError);
            Assert.AreEqual(0, tokenD.StartPosition.Position);
            Assert.AreEqual(6, tokenD.StopPosition.Position);
            Assert.AreEqual(123000.0, tokenD.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("123.0d3");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatDToken));
            tokenD = (FloatDToken)obj;
            Assert.IsTrue(tokenD.IsValid);
            Assert.IsNull(tokenD.ScanError);
            Assert.AreEqual(0, tokenD.StartPosition.Position);
            Assert.AreEqual(6, tokenD.StopPosition.Position);
            Assert.AreEqual(123000.0, tokenD.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("123.0e-2");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatEToken));
            token = (FloatEToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(7, token.StopPosition.Position);
            Assert.AreEqual(1.23F, token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_G()
        {
            // Float
            Scanner lexer = this.GetLexer("123.45e");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(FloatEToken));
            FloatEToken token = (FloatEToken)obj;
            Assert.IsFalse(token.IsValid);
            Assert.AreEqual("Invalid float. No digits found after exponent.", token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(7, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_H()
        {
            // Float - not really - just a test of integer, dot and an identifier
            Scanner lexer = this.GetLexer("123.e2");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(SmallIntegerToken));
            SmallIntegerToken token = (SmallIntegerToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(2, token.StopPosition.Position);
            Assert.AreEqual(123, token.Value);
            // The '.' token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(SpecialCharacterToken));
            // The 'e2' token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_I()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("123s");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ScaledDecimalToken));
            ScaledDecimalToken token = (ScaledDecimalToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(3, token.StopPosition.Position);
            Assert.AreEqual(token.Value.Scale, 0);
            Assert.AreEqual(token.Value, new BigDecimal(123));

            lexer = this.GetLexer("123.45s");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ScaledDecimalToken));
            token = (ScaledDecimalToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(6, token.StopPosition.Position);
            Assert.AreEqual(token.Value.Scale, 2);
            Assert.AreEqual(token.Value, new BigDecimal(12345, 2));

            lexer = this.GetLexer("123s4");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ScaledDecimalToken));
            token = (ScaledDecimalToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(4, token.StopPosition.Position);
            Assert.AreEqual(token.Value.Scale, 4);
            Assert.AreEqual(token.Value, new BigDecimal(123));

            lexer = this.GetLexer("123.45s4");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ScaledDecimalToken));
            token = (ScaledDecimalToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(7, token.StopPosition.Position);
            Assert.AreEqual(token.Value.Scale, 4);
            Assert.AreEqual(token.Value, new BigDecimal(12345, 2));
        }

        [TestMethod]
        public void Test_3_5_6_Numbers_J()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("123sabc");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(ScaledDecimalToken));
            ScaledDecimalToken token = (ScaledDecimalToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(3, token.StopPosition.Position);
            Assert.AreEqual(token.Value, new BigDecimal(123));

            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            Assert.AreEqual("abc", ((IdentifierToken)obj).Value);
        }

        [TestMethod]
        public void Test_3_5_7_Characters_A()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("$A");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CharacterToken));
            CharacterToken token = (CharacterToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            Assert.AreEqual('A', token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("$Ü");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CharacterToken));
            token = (CharacterToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            Assert.AreEqual('Ü', token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("$ ");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CharacterToken));
            token = (CharacterToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            Assert.AreEqual(' ', token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_7_Characters_B()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("$");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(CharacterToken));
            CharacterToken token = (CharacterToken)obj;
            Assert.IsFalse(token.IsValid);
            Assert.AreEqual("Missing character. Hit EOF.", token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_8_Strings_A()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("'Smalltalk'");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            StringToken token = (StringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(10, token.StopPosition.Position);
            Assert.AreEqual("Smalltalk", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("'Über\n\tSmalltalk'");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            token = (StringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(16, token.StopPosition.Position);
            Assert.AreEqual("Über\n\tSmalltalk", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }


        [TestMethod]
        public void Test_3_5_8_Strings_B()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("'Smalltalk '' Quotes'");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            StringToken token = (StringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(20, token.StopPosition.Position);
            Assert.AreEqual("Smalltalk ' Quotes", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("''");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            token = (StringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(1, token.StopPosition.Position);
            Assert.AreEqual("", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("''''");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            token = (StringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(3, token.StopPosition.Position);
            Assert.AreEqual("'", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_8_Strings_C()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("'Smalltalk ''");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(StringToken));
            StringToken token = (StringToken)obj;
            Assert.IsFalse(token.IsValid);
            Assert.AreEqual("Missing closing ' quote in string literal. Hit EOF.", token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(13, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_9_HashedStrings_A()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("#'Smalltalk'");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            HashedStringToken token = (HashedStringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(11, token.StopPosition.Position);
            Assert.AreEqual("Smalltalk", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("#'Über\n\tSmalltalk'");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            token = (HashedStringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(17, token.StopPosition.Position);
            Assert.AreEqual("Über\n\tSmalltalk", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_9_HashedStrings_B()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("#'Smalltalk '' Quotes'");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            HashedStringToken token = (HashedStringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(21, token.StopPosition.Position);
            Assert.AreEqual("Smalltalk ' Quotes", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("#''");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            token = (HashedStringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(2, token.StopPosition.Position);
            Assert.AreEqual("", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("#''''");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            token = (HashedStringToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(4, token.StopPosition.Position);
            Assert.AreEqual("'", token.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_9_HashedStrings_C()
        {
            // Scaled decimals
            Scanner lexer = this.GetLexer("#'Smalltalk ''");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(HashedStringToken));
            HashedStringToken token = (HashedStringToken)obj;
            Assert.IsFalse(token.IsValid);
            Assert.AreEqual("Missing closing ' quote in string literal. Hit EOF.", token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(14, token.StopPosition.Position);
        }

        [TestMethod]
        public void Test_3_5_10_Selectors_A()
        {
            Scanner lexer = this.GetLexer("#isGood1Time:");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(QuotedSelectorToken));
            QuotedSelectorToken token = (QuotedSelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(12, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time:", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));

            lexer = this.GetLexer("#isGood1Time ");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(QuotedSelectorToken));
            token = (QuotedSelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(11, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(WhitespaceToken));

            lexer = this.GetLexer("#with:with: ");
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(QuotedSelectorToken));
            token = (QuotedSelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(10, token.StopPosition.Position);
            Assert.AreEqual("with:with:", token.Value);
            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(WhitespaceToken));
        }

        [TestMethod]
        public void Test_3_5_10_Selectors_B()
        {
            // An unadorned identifier is an identifier which is not immediately preceded by a '#'. 
            // If a ':' followed by an '=' immediately follows an unadorned identifier, 
            // with no intervening white space, then the token is to be parsed as an
            // identifier followed by an assignmentOperator not as an keyword followed by an '='.
            Scanner lexer = this.GetLexer("#isGood1Time:=");
            // Identifier token
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(QuotedSelectorToken));
            QuotedSelectorToken token = (QuotedSelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(12, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time:", token.Value);
            // Assignment token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(BinarySelectorToken));
            BinarySelectorToken equals = (BinarySelectorToken)obj;
            Assert.IsTrue(equals.IsValid);
            Assert.IsNull(equals.ScanError);
            Assert.AreEqual(13, equals.StartPosition.Position);
            Assert.AreEqual(13, equals.StopPosition.Position);
            Assert.AreEqual("=", equals.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_10_Selectors_C()
        {
            // An unadorned identifier is an identifier which is not immediately preceded by a '#'. 
            // If a ':' followed by an '=' immediately follows an unadorned identifier, 
            // with no intervening white space, then the token is to be parsed as an
            // identifier followed by an assignmentOperator not as an keyword followed by an '='.
            Scanner lexer = this.GetLexer("#isGood1Time:true");
            // Identifier token
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(QuotedSelectorToken));
            QuotedSelectorToken token = (QuotedSelectorToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(12, token.StopPosition.Position);
            Assert.AreEqual("isGood1Time:", token.Value);
            // Assignment token
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(IdentifierToken));
            IdentifierToken idf = (IdentifierToken)obj;
            Assert.IsTrue(idf.IsValid);
            Assert.IsNull(idf.ScanError);
            Assert.AreEqual(13, idf.StartPosition.Position);
            Assert.AreEqual(16, idf.StopPosition.Position);
            Assert.AreEqual("true", idf.Value);

            // Should be the last one
            obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(EofToken));
        }

        [TestMethod]
        public void Test_3_5_11_Spaces_C()
        {
            Scanner lexer = this.GetLexer(" \t \r \n   ");
            object obj = lexer.GetToken();
            Assert.IsInstanceOfType(obj, typeof(WhitespaceToken));
            WhitespaceToken token = (WhitespaceToken)obj;
            Assert.IsTrue(token.IsValid);
            Assert.IsNull(token.ScanError);
            Assert.AreEqual(0, token.StartPosition.Position);
            Assert.AreEqual(8, token.StopPosition.Position);
        }

        public Scanner GetLexer(string str)
        {
            Scanner scanner = new Scanner(new StringReader(str));
            return scanner;
        }
    }
}
