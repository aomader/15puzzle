module FifteenPuzzleTests.Solver

open Xunit
open FsCheck
open FsUnit.Xunit
open System

open FifteenPuzzle.Solver

(*
let scramble (sqn : seq<'T>) = 
    let rnd = new Random()
    let rec scramble2 (sqn : seq<'T>) = 
        /// Removes an element from a sequence.
        let remove n sqn = sqn |> Seq.filter (fun x -> x <> n)
 
        seq {
            let x = sqn |> Seq.nth (rnd.Next(0, sqn |> Seq.length))
            yield x
            let sqn' = remove x sqn
            if not (sqn' |> Seq.isEmpty) then
                yield! scramble2 sqn'
        }
    scramble2 sqn
    *)

let goal = [1..15] @ [0]

type BoardArbitrary =
    static member Board() =
        Arb.fromGen  (gen { return (shuffle goal 200) })

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``2 <= |children b| <= 4`` b =
    let n = Seq.length (children b)
    n >= 2 && n <= 4

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``|nub (children b)| == |children b|`` b =
    let nub = Set.ofList >> Set.toList
    let bs = children b
    Seq.length bs = Seq.length (nub bs)

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``distance a a = 0`` a =
    distance a a = 0

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``distance a b >= 0`` a b =
    distance a b >= 0

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``distance a b > 0 if a != b`` a b =
    match a = b with
      | false -> distance a b > 0
      | _     -> true

[<Property( Arbitrary=[| typeof<BoardArbitrary> |] )>]
let ``|solve a a| = 1`` a =
    Seq.length (solve a a) = 1
