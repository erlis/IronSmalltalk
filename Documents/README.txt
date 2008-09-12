IronSmalltalk v0.1 Documentation

----- CONTENTS -----
A. INTRODUCTION
B. SYNTAX
C. PRIMITIVE TYPES
D. PRIMITIVE TYPE SELECTORS

A. INTRODUCTION

IronSmalltalk is the implementation of the Smalltalk language on the DLR.  It is currently being written by Trey Tomes.  You can find information about this project at http://www.codeplex.com/IronSmalltalk.

Open the IronSmalltalk solution, and run IronSmalltalk.Test to see 

B. SYNTAX

Though the project is still in its infancy, it can accept a small subset of Smalltalk syntax.

The following BNF grammar should be followed:

<ExpressionSequence> := <Expression> [ \. <ExpressionSequence> ] \.?
<Expression> := <Receiver> <MessageSequence>?
<MessageSequence> := <Message> [ \; <MessageSequence> ]
<Message> := <Name>

This is not the complete Smalltalk grammar, and it will be expanded as the project continues.

Examples:

At the prompt, type the following:

> $C. [Enter]

The value '$C' will be returned to the console.  This syntax is true of any Smalltalk object.

> $C asLowercase. [Enter]

The value '$c' will be returned to the console.  The asUppercase also works as expected.

> 16rF negated. [Enter]

The value '-15' will be returned to the console.

> 'Hello world!' size; negated.

The value '-12' will be returned to the console.  First, the 'size' selector is sent to the string object, returning an integer object, then the 'negated' selector is sent to the integer object.

C. PRIMITIVE TYPES

IronSmalltalk recognizes the following primitive types:
1. SmallCharacter, defined by the regular expression "\$.".
2. SmallFloat, defined by the regular expression "([0-9]+r)?[-+]?[0-9A-Z]*\.[0-9A-Z]+(e[0-9]+)?".
3. SmallInteger, defined by the regular expression "([0-9]+r)?[-+]?[0-9A-Z]+(e[0-9]+)?".
4. SmallString, defined by the regular expression "'(?:[^']|'')*'".
5. SmallSymbol, defined by the regular expression "#'(?:[^']|'')*'".

D. PRIMITIVE TYPE SELECTORS

A subset of selectors are available to these types.

SmallCharacter
--------------
value "answers the value of the character"
asLowercase "answers the lowercase value of the character"
asUppercase "answers the uppercase value of the character"
next "answers the next character in sequence, i.e. $A next := $B"

SmallInteger
------------
value "answers the value of the integer"
negated "answers the value of the integer * -1"

SmallString
-----------
value "answers the value of the string"
size "answers the character length of the string"
