namespace FifteenPuzzle.ViewModel

open System
open System.Windows.Input
open System.Collections
open FifteenPuzzle.Solver

type SolvingState =
    Unsolved | Solving | Solved

/// The backing ViewModel of the MainWindow
type MainWindowViewModel() =
    inherit BaseViewModel()

    // fields
    let mutable state = Unsolved
    let mutable board =  [1..15] @ [0]
    let mutable solution = []
    let mutable index = 0
    let mutable solvedStates = ""

    let updateBoard (x: MainWindowViewModel) = do
        x.Board <- solution.[index]
        x.SolvedStates <- sprintf "%d / %d" (index + 1) (Seq.length solution)
        x.OnPropertyChanged "PreviousCommand"
        x.OnPropertyChanged "NextCommand"

    // properties
    member x.State
        with get() = state
        and set(value) =
            state <- value
            x.OnPropertyChanged "State"

    member x.Board
        with get() = board
        and set(value) =
            board <- value
            x.OnPropertyChanged "Board"

    member x.SolvedStates
        with get() = solvedStates
        and set(value) =
            solvedStates <- value
            x.OnPropertyChanged "SolvedStates"

    // commands
    member x.StepCommand =
        RelayCommand(fun o ->
                         index <- index + (unbox<string> >> int) o
                         x.Board <- solution.[index] )

    member x.ShuffleCommand =
        RelayCommand(fun o ->
                         x.Board <- shuffle x.Board 200
                         x.SolvedStates <- ""
                         x.State <- Unsolved
                         x.OnPropertyChanged "SolveCommand" )

    member x.SolveCommand =
        RelayCommand((fun o -> 
                         x.State <- Solving
                         Async.Start (async {
                             solution <- solve x.Board ([1..15] @ [0])
                             index <- 0
                             updateBoard x
                             x.State <- Solved })),
                     (fun o ->
                         x.Board <> ([1..15] @ [0])) )

    member x.PreviousCommand =
        RelayCommand((fun o ->
                         index <- index - 1
                         updateBoard x),
                     (fun o ->
                         index > 0) )

    member x.NextCommand =
        RelayCommand((fun o ->
                         index <- index + 1
                         updateBoard x),
                     (fun o ->
                         index < Seq.length solution - 1) )