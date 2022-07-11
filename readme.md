# Parser Syntax

## Buttons

```
a
b
```


## Sections

```
a
b
--
c
d
```


```
a
b--
c
d
```

## Compound Buttons

Use the `#(label[, color])` syntax to declare compound buttons which allow other button properties to be configured.

Compound button syntax requires use of the escaping construct `#(...)` which can then contain the following properties:

- label - the name to display for the button
- color - one of the permitted known colors.  If a non-known color is specified, then the color will revert to a default color.

Examples

```
A red button with a label of 'A'
#(A, Red)

A button labelled 'B' with the default color applied.  This is equivalent to using non-compound syntax
#(B)

A button labelled 'C' with the default color applied.
#(C, NonExistantColor)
```


## Columns

The following example uses the `|` character to declare a second column in the first group. The form will display buttons 'a' and 'b' in the first column, buttons 'c' and 'd' will then appear in a second column to the right.

```
a
b
|
c
d
--
e
f
```

## Known Colors

The following color palette can be used to configure buttons.

- Blue (default)
- Red
- Green
- Gray
- Black



