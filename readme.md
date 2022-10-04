# Forms Parser

Forms parser was written as a way to provide configurable forms for [Matchlib.com](https://matchlib.com).

![Rendered form](https://github.com/dneimke/forms-parse/blob/main/images/full-example-form.png?raw=true)

The component separates the task of parsing syntax from the host application and returns an object graph from provided source.

The following sections explain the syntax for creating forms.


## Buttons

Creates 2 buttons on the same row named 'a' and 'b'.

```
a
b
```


## Rows

Buttons can be grouped into rows using the `--` delimiter. The following example shows 4 buttons grouped into 2 separate rows. 

```
a
b
--
c
d
```

It is important to place the row delimiter on a separate row. The following example shows an _invalid_ syntax where only a single row is created for all 4 buttons.

**Invalid row syntax**
```
a
b--
c
d
```

## Compound Buttons

Use the `#(name[, color, label])` syntax to declare compound buttons which allow other button properties to be configured.

You can also use named attributes which allows you to specify attributes in any order. The following 2 declarations produce the same button:

```
#(D, blue, switch)
#(name: D, type: switch, color: blue)
```

Compound button syntax requires use of the escaping construct `#(...)` which can then contain the following properties:

- name - the name to display for the button
- color - one of the permitted known colors.  If a non-known color
- type - allows you to specify the button type from the following valid types - `label`, `button`, and `switch`. This is an optional value and will default to `Button` if not provided or if the value provided does not match any of the valid types.

**Examples**

```
A red button with a label of 'A'
#(A, Red)

A red button with a label of 'A' using named parameters
#(name: A, color: Red)

A button named 'B' with the default color applied.  This is equivalent to using non-compound syntax
#(B)

A label named 'C' with the Blue color applied.
#(D, Blue, Label)

A switch named 'D' with the Green color applied.
#(name: D, color: blue, type: switch)
```

**Note**: when using named parameters, attributes can be specified in any order. 

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

## Detailed Example

The following form definition:

```
Press
Outlet
|
#(Pocket, Blue, Label)
#(Corridor, Blue, Label)
#(Advanced, Blue, Label)
#(Review, Blue, Label)
--
Att. 25 Entry
Def. 25 Entry
|
#(Side, Blue, Label)
#(Middle, Blue, Label)
--
Att. Circle Entry
Def. Circle Entry
|
#(Pass, Blue, Label)
#(Dribble, Blue, Label)
#(Through, Blue, Label)
--
Shot For
Shot Ag
--
Goal For
Goal Ag
|
#(Field, Blue, Label)
#(APC, Blue, Label)
#(Stroke, Blue, Label)
#(1v1, Blue, Label)
--
APC
DPC
Stroke
1v1
|
#(Flick, Blue, Label)
#(Hit, Blue, Label)
#(Other, Blue, Label)
--
#(Special, Red)
#(Card, Green)
#(Injury, Green)
--
#(name: Possession For, type: switch)
#(name: Possession Ag, type: switch)
```

Generates this coded form:

![Rendered form](https://github.com/dneimke/forms-parse/blob/main/images/full-example-form.png?raw=true)
