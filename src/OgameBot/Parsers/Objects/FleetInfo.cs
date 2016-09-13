using System;
using OgameBot.Objects;
using OgameBot.Objects.Types;
using ScraperClientLib.Engine;

namespace OgameBot.Parsers.Objects
{
    public class FleetInfo : DataObject
    {
        public int Id { get; set; }

        public DateTimeOffset ArrivalTime { get; set; }

        public bool IsReturning { get; set; }

        public MissionType MissionType { get; set; }

        public FleetEndpointInfo Origin { get; set; }

        public FleetEndpointInfo Destination { get; set; }

        public FleetComposition Composition { get; set; }

        public override string ToString()
        {
            if (IsReturning)
                return $"Fleet {Id} ({MissionType}), {Origin.Coordinate} <- {Destination.Coordinate}, at: {ArrivalTime} - ships: {Composition.TotalShips:N0}";

            return $"Fleet {Id} ({MissionType}), {Origin.Coordinate} -> {Destination.Coordinate}, at: {ArrivalTime} - ships: {Composition.TotalShips:N0}";
        }
    }
}