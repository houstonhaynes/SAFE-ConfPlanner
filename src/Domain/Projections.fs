module Projections

open System
open Model
open Events

let updateAbstractStatus abstractId status (abstr: ConferenceAbstract) =
    match abstr.Id = abstractId with
    | true -> { abstr with Status = status }
    | false -> abstr

let apply (conference : Conference) event : Conference =
  match event with
    | ConferenceScheduled conference ->
        conference

    | OrganizerRegistered o ->
        { conference with Organizers = o :: conference.Organizers }

    | TalkWasProposed t ->
        { conference with Abstracts = t :: conference.Abstracts }

    | CallForPapersOpened ->
        { conference with CallForPapers = Open }

    | CallForPapersClosed ->
        { conference with CallForPapers = Closed; VotingPeriod = InProgess }

    | NumberOfSlotsDecided i ->
        { conference with AvailableSlotsForTalks = i }

    | AbstractWasProposed proposed ->
        { conference with Abstracts = proposed :: conference.Abstracts }

    | AbstractWasAccepted abstractId ->
        { conference with Abstracts = conference.Abstracts |> List.map (updateAbstractStatus abstractId AbstractStatus.Accepted) }

    | AbstractWasRejected abstractId ->
        { conference with Abstracts = conference.Abstracts |> List.map (updateAbstractStatus abstractId AbstractStatus.Rejected) }

    | VotingPeriodWasFinished ->
        { conference with VotingPeriod = Finished }

    | VotingPeriodWasReopened ->
        { conference with
            VotingPeriod = InProgess
            Abstracts = conference.Abstracts |> List.map (fun abstr -> { abstr with Status = AbstractStatus.Proposed }) }

    | VotingWasIssued voting ->
        { conference with Votings = voting :: conference.Votings }

    | FinishingDenied _ ->
        conference

    | VotingDenied _ ->
        conference

    | ProposingDenied _ ->
        conference

    | ConferenceAlreadyScheduled ->
        conference

let private emptyConference : Conference =
  {
    Id = ConferenceId <| Guid.Empty
    CallForPapers = NotOpened
    VotingPeriod = InProgess
    Abstracts = List.empty
    Votings = List.empty
    Organizers = List.empty
    AvailableSlotsForTalks = 0
  }

let conferenceState (givenHistory : Event list) =
    givenHistory
    |> List.fold apply emptyConference
