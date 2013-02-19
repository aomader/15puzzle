namespace FifteenPuzzle.Converter

open Microsoft.FSharp.Reflection
open System
open System.Windows
open System.Windows.Data

/// Compares union cases and returns Visible on equality
type UnionToVisibilityConverter() =
    interface IValueConverter with
        member x.Convert(v, t, p, c) =
            match p with
                | :? string as parameter -> match FSharpType.GetUnionCases (v.GetType()) |> Array.filter (fun case -> case.Name = parameter) with
                                                | [|case|] -> box<Visibility> (if (FSharpValue.MakeUnion(case, [||]).Equals(v))
                                                                                   then Visibility.Visible
                                                                                   else Visibility.Hidden)
                                                | _        -> DependencyProperty.UnsetValue
                | _ -> DependencyProperty.UnsetValue
        member x.ConvertBack(o, t, p, c) = raise (NotSupportedException())
