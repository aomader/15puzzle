namespace FifteenPuzzle.Converter

open Microsoft.FSharp.Reflection
open System
open System.Windows
open System.Windows.Data

/// Compares ints and returns visible on inequality
type NumberToVisibilityConverter() =
    interface IValueConverter with
        member x.Convert(v, t, p, c) =
            box<Visibility> (if unbox<int> v = Convert.ToInt32(p :?> string, 10)
                                 then Visibility.Hidden
                                 else Visibility.Visible)
        member x.ConvertBack(o, t, p, c) = raise (NotSupportedException())
