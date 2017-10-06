module Conference.Types

open Global
open Infrastructure.Types
open Server.ServerTypes

type Msg =
  | Received of ServerMsg<Events.Event,ConferenceApi.QueryResult>
  | QueryState
  | FinishVotingperid


type Model =
  {
    State : RemoteData<Model.Conference>
  }
