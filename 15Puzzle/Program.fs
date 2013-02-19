module Program

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Markup
open FifteenPuzzle.ViewModel

[<STAThread>]
[<EntryPoint>]
let main argv =
  let application = Application.LoadComponent(new Uri("Application.xaml", UriKind.Relative)) :?> Application
  application.Run()