module Conference.Types

open Global
open Infrastructure.Types
open Server.ServerTypes
open Model
open Conference.Api

type Msg =
  | Received of ServerMsg<Events.Event,API.QueryResult>
  | Vote of Voting
  | FinishVotingperiod
  | ToggleMode
  | ReopenVotingperiod
  | MakeItSo
  | SwitchToConference of ConferenceId

type WhatIf =
  {
    Conference : Model.Conference
    Commands : Command<Commands.Command> list
    Events : Events.Event list
  }


type Mode =
  | Live
  | WhatIf of WhatIf

type Model =
  {
    Conference : RemoteData<Model.Conference*Mode>
    Conferences : RemoteData<API.Conferences>
    LastEvents : Events.Event list
    Organizer : OrganizerId
  }
