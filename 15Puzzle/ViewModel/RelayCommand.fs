namespace FifteenPuzzle.ViewModel

open System
open System.Windows.Input

type RelayCommand(execute: (obj -> unit), ?canExecute0: (obj -> bool)) =
    let canExecute = defaultArg canExecute0 (fun _ -> true)
    let canExecuteChangedEvent = new Event<_,_>()

    interface ICommand with
        member x.CanExecute o = canExecute o
        member x.Execute o = execute o
        [<CLIEvent>]
        member x.CanExecuteChanged = canExecuteChangedEvent.Publish