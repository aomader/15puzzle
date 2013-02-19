namespace FifteenPuzzle.ViewModel

open System
open System.Windows
open System.Windows.Input
open System.ComponentModel

type BaseViewModel() =
    let propertyChangedEvent = new DelegateEvent<PropertyChangedEventHandler>()

    interface INotifyPropertyChanged with
        [<CLIEvent>]
        member this.PropertyChanged = propertyChangedEvent.Publish

    member this.OnPropertyChanged propertyName = 
        propertyChangedEvent.Trigger([| this; new PropertyChangedEventArgs(propertyName) |])