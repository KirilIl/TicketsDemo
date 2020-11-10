﻿using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using TicketsDemo.Data.Entities;
using TicketsDemo.Data.Repositories;

namespace TicketsDemo.MongoDb.Repositories
{
    public class MongoDbTrainRepository : ITrainRepository
    {

        #region ITrainRepository Members

        public List<Train> GetAllTrains()
        {
            var ctx = new TicketsContext();
            var trains = ctx.Trains.Find(new BsonDocument()).ToList();

            return InitializeTrains(trains);
        }

        public Train GetTrainDetails(int id)
        {
            var ctx = new TicketsContext();
            var train = ctx.Trains.Find(new BsonDocument("_id", id)).FirstOrDefault();

            foreach (var c in train.Carriages)
            {
                foreach (var p in c.Places)
                {
                    p.Carriage = c;
                }

                c.Train = train;
            }

            return train;
        }

        public void CreateTrain(Train train)
        {
            var ctx = new TicketsContext();
            ctx.Trains.InsertOne(train);
        }

        public void UpdateTrain(Train train)
        {
            var ctx = new TicketsContext();
            ctx.Trains.ReplaceOne(new BsonDocument("_id", train.Id), train);
        }

        public void DeleteTrain(Train train)
        {
            var ctx = new TicketsContext();
            ctx.Trains.DeleteOne(new BsonDocument("_id", train.Id));
        }

        #endregion

        private List<Train> InitializeTrains(List<Train> trains)
        {
            foreach (var t in trains)
            {
                foreach (var c in t.Carriages)
                {
                    foreach (var p in c.Places)
                    {
                        p.Carriage = c;
                    }

                    c.Train = t;
                }
            }

            return trains;
        }
    }
}
