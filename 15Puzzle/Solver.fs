module FifteenPuzzle.Solver

open System
open System.Collections.Generic

// The solver supports arbitrary large puzzles
let mutable rows : int = 4
let mutable columns : int = 4

/// A state describing the board
type board = int list

/// Translate an 1d index into the 2d column index
let column : int -> int = fun i ->
    i % columns

/// Translate an 1d index into the 2d row index
let row : int -> int = fun i ->
    i / columns

/// Translate a 2d index composed of x and y value into an 1d index
let index : int * int -> int = fun (c, r) ->
    r * rows + c

/// Generate all possible sub board states  
let children : board -> board list = fun b ->
    let i = Seq.findIndex ((=) 0) b
    let x, y = column i, row i
    let flip f x y = f y x
    [(x-1, y); (x+1, y); (x, y-1); (x, y+1)]
        |> Seq.filter (fun (c, r) -> c >= 0 && r >= 0 && c < columns && r < rows)
        |> Seq.map (index >> flip Seq.nth b)
        |> Seq.map (fun n -> List.map (fun i -> match i with
                                                    | 0            -> n
                                                    | x when x = n -> 0
                                                    |            _ -> i) b)
        |> Seq.toList

/// Creates a new board by applying n random moves
/// to the board to preserve the solvability
let rec shuffle : board -> int -> board = fun b n ->
    let rnd = Random()
    Seq.fold (fun b _ -> let c = children b
                         Seq.nth (rnd.Next(0, Seq.length c)) c) b (seq {1..n})

/// Measure the distance between two boards using Manhatten distance
let distance : board -> board -> int = fun a b ->
    a
        |> Seq.mapi (fun i v -> (i, Seq.findIndex ((=) v) b))
        |> Seq.map (fun (i, j) -> abs (column i - column j) + abs (row i - row j))
        |> Seq.sum

/// A node inside the search tree
type State =
    { Parent: State option;
      Board: board;
      Level: int;
      Score: int; }

/// Tries to solve the puzzle defined by the start and end state using
/// an A* search based on Manhatten distance as h and the depth as g
let solve : board -> board -> board list = fun start goal ->
    let tested = SortedSet<board>()
    let space = SortedList<State, State>({ new IComparer<State> with
                                             member x.Compare(a, b) = match a.Score.CompareTo(b.Score) with
                                                                          | 0 -> -1
                                                                          | c -> c })

    let rec search() =
        let candidate = space.Values.[0]

        tested.Add(candidate.Board) |> ignore
        space.RemoveAt(0)

        if candidate.Board = goal then
            candidate
        else
            let a = children candidate.Board
            let children =
                children candidate.Board
                    |> Seq.filter (fun b -> tested.Contains(b) |> not)
                    |> Seq.map (fun b -> { Parent = Some candidate;
                                            Board = b;
                                            Level = candidate.Level + 1;
                                            Score = (candidate.Level + 1) + 2 * (distance b goal); })

            for c in children do
                space.Add(c, c)

            search()

    let s = { Parent = None; Board = start; Level = 0; Score = distance start goal; }
    space.Add(s, s)

    search() |> Some
        |> Seq.unfold (fun s ->  match s with
                                     | None    -> None
                                     | Some s' -> Some (s'.Board, s'.Parent))
        |> Seq.toList |> List.rev