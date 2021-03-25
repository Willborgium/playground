using System;
using System.Collections.Generic;
using System.Linq;

namespace SimulationEngine
{
    public class AgentFactory
    {
        public IEnumerable<Agent> CreateAgents(int count) =>
            Enumerable
                .Range(0, count)
                .Select(CreateAgent)
                .ToList();

        private Agent CreateAgent(int index)
        {
            var attributeCount = _random.Next(0, AttributeNames.Length);
            var attributes = AttributeNames
                .TakeRandom(attributeCount)
                .Select(CreateAttribute);
            return new Agent(attributes);
        }

        private Attribute CreateAttribute(string name)
        {
            return new Attribute(name, _random.Next(1, 1000));
        }

        private static readonly string[] AttributeNames =
        {
            "a",
            "b",
            "c",
            "d",
            "e",
            "f"
        };

        private readonly Random _random = new Random();
    }
}
