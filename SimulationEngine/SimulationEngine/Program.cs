using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SimulationEngine
{
    [DebuggerDisplay("{Value}", Name = "{Name}")]
    public class Attribute
    {
        public string Name { get; }
        public int Value { get; set; }

        public Attribute(string name, int value)
        {
            Name = name;
            Value = value;
        }
    }

    public class Motivation
    {
        public string TargetAttributeName { get; }
        public int TargetValue { get; }
        public int Priority { get; set; }

        public Motivation(string targetAttributeName, int targetValue, int priority)
        {
            TargetAttributeName = targetAttributeName;
            TargetValue = targetValue;
            Priority = priority;
        }
    }

    public class Effect
    {
        public string TargetAttributeName { get; }
        public int TargetValue { get; }
        public int Priority { get; set; }
        public Effect(string targertAtributeName, int targetValue, int priority)
        {
            TargetAttributeName = targertAtributeName;
            TargetValue = targetValue;
            Priority = priority;
        }
    }

    public class Agent
    {
        public ICollection<Attribute> Attributes { get; }

        public ICollection<Motivation> Motivations { get; }
        public ICollection<Effect> Effects { get; }

        public Agent(IEnumerable<Attribute> attributes)
        {
            Attributes = new List<Attribute>(attributes);
            Motivations = new List<Motivation>();
            Effects = new List<Effect>();
        }
    }

    public class Program
    {
        public static void Main()
        {
            var agentFactory = new AgentFactory();
            var agents = agentFactory.CreateAgents(100);

            for (var iterationIndex = 0; iterationIndex < 1000; iterationIndex++)
            {
                foreach (var agent in agents)
                {
                    // process effects

                    var processedMotivations = agent.Motivations.OrderByDescending(m => m.Priority).Take(2).ToList();
                    foreach (var motivation in processedMotivations)
                    {
                        agent.Motivations.Remove(motivation);
                        agent.Effects.Add(new Effect(motivation.TargetAttributeName, motivation.TargetValue, motivation.Priority));
                    }
                }
            }
        }
    }
}
