module Conference.View

open Conference.Types

open System
open Elmish
open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Browser
open Fable.Helpers.React
open Fable.Helpers.React.Props

open Infrastructure.Types
open Server.ServerTypes
open Global
open Model

let name (speaker:Speaker) =
  String.concat speaker.Lastname

let talk (t:Model.ConferenceAbstract) =
    div [] [ t.Text |> str ]

let simpleButton txt action dispatch =
  div
    [ ClassName "column" ]
    [ a
        [ ClassName "button"
          OnClick (fun _ -> action |> dispatch) ]
        [ str txt ] ]


let abstractColumn color filter conference  =
  let abstracts =
    conference.Abstracts
    |> List.filter filter
    |> List.map talk

  div
    [
      ClassName "column"
      Style
        [
          BackgroundColor color
          Display Flex
          FlexDirection "column"
        ]
    ]
    abstracts

let proposedColumn =
  abstractColumn "#dddddd" (fun abs -> abs.Status = Proposed)

let acceptedColumn =
  abstractColumn "#ddffdd" (fun abs -> abs.Status = Accepted)

let rejectedColumn =
  abstractColumn "#ffdddd" (fun abs -> abs.Status = Rejected)

let viewAbstracts conference dispatch =
  div []
    [
      div [ ClassName "columns is-vcentered" ]
        [
          match conference.VotingPeriod with
          | InProgess ->
              yield simpleButton "Finish Votingperiod" FinishVotingperid dispatch

          | Finished ->
              yield  div [ ClassName "column"] [ "Voting Period already Finished" |> str ]
        ]

      div [ ClassName "columns is-vcentered" ]
        [
          div [ ClassName "column"] [ "Proposed" |> str ]
          div [ ClassName "column"] [ "Accepted" |> str ]
          div [ ClassName "column"] [ "Rejected" |> str ]
        ]

      div [ ClassName "columns is-vcentered" ]
        [
          proposedColumn conference
          acceptedColumn conference
          rejectedColumn conference
        ]
    ]

let root (model: Model) dispatch =
  match model.State with
     | Success conference ->
         viewAbstracts conference dispatch

     | _ ->
        div [ ClassName "columns is-vcentered" ]
          [
            div [ ClassName "column"] [ "State Not Loaded" |> str ]
          ]
