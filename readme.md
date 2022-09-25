# Forms Parser

Forms parser was written as a way to provide configurable forms for [Matchlib.com](https://matchlib.com).

![Rendered form](https://github.com/dneimke/forms-parse/blob/main/images/full-example-form.png?raw=true)

The component separates the task of parsing syntax from the host application and returns an object graph from provided source.

The following sections explain the syntax for creating forms.


## Buttons

```
a
b
```


## Rows

Buttons can be grouped into rows by using the `--` delimiter. The following example shows 4 buttons grouped into 2 separate rows. 

```
a
b
--
c
d
```

It is important to place the row delimiter on a separate row. The following example shows an invalid syntax where only a single row is created for all 4 buttons.

**Invalid row syntax**
```
a
b--
c
d
```

## Compound Buttons

Use the `#(name[, color, label])` syntax to declare compound buttons which allow other button properties to be configured.

Compound button syntax requires use of the escaping construct `#(...)` which can then contain the following properties:

- name - the name to display for the button
- color - one of the permitted known colors.  If a non-known color
- label - allows you to specify either `Label` or `Button`. This is an optional value and will default to `Button` if not provided.

**Examples**

```
A red button with a label of 'A'
#(A, Red)

A button named 'B' with the default color applied.  This is equivalent to using non-compound syntax
#(B)

A button named 'C' with the default color applied.
#(C, NonExistantColor)

A label named 'D' with the Blue color applied.
#(D, Blue, Label)
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
```

Generates this coded form:

![Rendered form](https://github.com/dneimke/forms-parse/blob/main/images/full-example-form.png?raw=true)
