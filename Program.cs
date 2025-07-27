using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

/**
 * Win the water fight by controlling the most territory, or out-soak your opponent!
 **/
class Player
{
    class Agent
    {
        public int AgentId { get; set; }
        public int PlayerId { get; set; }
        public int ShootCoolDown { get; set; }
        public int OptimalRange { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Wetness { get; set; }

    }

    class Position
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int TileType { get; set; }
    }
    static void Main(string[] args)
    {
        string[] inputs;
        Dictionary<int, Agent> agents = new();
        int myId = int.Parse(Console.ReadLine()); // Your player id (0 or 1)
        int agentDataCount = int.Parse(Console.ReadLine()); // Total number of agents in the game
        for (int i = 0; i < agentDataCount; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            int agentId = int.Parse(inputs[0]); // Unique identifier for this agent
            int player = int.Parse(inputs[1]); // Player id of this agent
            int shootCooldown = int.Parse(inputs[2]); // Number of turns between each of this agent's shots
            int optimalRange = int.Parse(inputs[3]); // Maximum manhattan distance for greatest damage output
            int soakingPower = int.Parse(inputs[4]); // Damage output within optimal conditions
            int splashBombs = int.Parse(inputs[5]); // Number of splash bombs this can throw this game
            if (!agents.TryGetValue(agentId, out var storedAgent))
            {
                agents[agentId] = new Agent()
                {
                    AgentId = agentId,
                    PlayerId = player,
                    ShootCoolDown = shootCooldown,
                    OptimalRange = optimalRange
                };

            }

        }
        inputs = Console.ReadLine().Split(' ');
        int width = int.Parse(inputs[0]); // Width of the game map
        int height = int.Parse(inputs[1]); // Height of the game map
        List<Position> grid = new();
        for (int i = 0; i < height; i++)
        {
            inputs = Console.ReadLine().Split(' ');
            for (int j = 0; j < width; j++)
            {
                int x = int.Parse(inputs[3 * j]);// X coordinate, 0 is left edge
                int y = int.Parse(inputs[3 * j + 1]);// Y coordinate, 0 is top edge
                int tileType = int.Parse(inputs[3 * j + 2]);
                grid.Add(new Position() { X = x, Y = y, TileType = tileType });
            }
        }

        // game loop
        while (true)
        {
            int agentCount = int.Parse(Console.ReadLine()); // Total number of agents still in the game
            for (int i = 0; i < agentCount; i++)
            {
                inputs = Console.ReadLine().Split(' ');
                int agentId = int.Parse(inputs[0]);
                int x = int.Parse(inputs[1]);
                int y = int.Parse(inputs[2]);
                int cooldown = int.Parse(inputs[3]); // Number of turns before this agent can shoot
                int splashBombs = int.Parse(inputs[4]);
                int wetness = int.Parse(inputs[5]); // Damage (0-100) this agent has taken
                Console.Error.WriteLine($"{agentId}");
                if (agents.TryGetValue(agentId, out var storedAgent))
                {
                    storedAgent.Wetness = wetness;
                    storedAgent.X = x;
                    storedAgent.Y = y;
                }

            }

            var moreWetAgent = agents.Values.OrderByDescending(o => o.Wetness).Where(o => o.PlayerId != myId && o.Wetness < 100).FirstOrDefault();
            Console.Error.WriteLine($"{moreWetAgent.Wetness} {moreWetAgent.AgentId}");
            var myAgents = agents.Values.Where(o => o.PlayerId == myId).ToList();
            int myAgentCount = int.Parse(Console.ReadLine()); // Number of alive agents controlled by you
            for (int i = 0; i < myAgentCount; i++)
            {
                var myAgent = myAgents[i];


                int indexToAttack = -1;
                int index = 0;
                int manhattanDistance = Math.Abs(myAgent.X - moreWetAgent.X) + Math.Abs(myAgent.Y - moreWetAgent.Y);


                if (manhattanDistance < myAgent.OptimalRange || manhattanDistance <= myAgent.OptimalRange * 2)
                {
                    Console.WriteLine($"{myAgent.AgentId};SHOOT {moreWetAgent.AgentId}");
                }
                else
                {
                    Console.WriteLine($"{myAgent.AgentId};MOVE {moreWetAgent.X} {moreWetAgent.Y}");
                }
                agents.Remove(moreWetAgent.AgentId);
                // Write an action using Console.WriteLine()
                // To debug: Console.Error.WriteLine("Debug messages...");

                // if(i == 0){
                //     Console.WriteLine("1;MOVE 6 1");
                // }
                // else{
                //     Console.WriteLine("2;MOVE 6 3");
                // }
                // One line per agent: <agentId>;<action1;action2;...> actions are "MOVE x y | SHOOT id | THROW x y | HUNKER_DOWN | MESSAGE text"
                //Console.WriteLine("HUNKER_DOWN");
            }
        }
    }

    // public static int GetCover(Agent agent, Agent opponent)
    // {
    //     var cover = 0;
    //     if(agent.Col+2<opponent.Col || 
    //     agent.Col+2 == opponent.Col && 2 <= Math.Abs(agent.Row-opponent.Row))
    //     cover = Math.Max(cover, Cover[agent.Row, agent.Col+1])

    //     if(opponent.Col+2<agent.Col || 
    //     opponent.Col+2 == agent.Col && 2 <= Math.Abs(agent.Row-opponent.Row))
    //     cover = Math.Max(cover, Cover[agent.Row, agent.Col-1])

    //        if(opponent.Row+2<agent.Row || 
    //         opponent.Row+2 == agent.Row && 2 <= Math.Abs(agent.Col-opponent.Col))
    //     cover = Math.Max(cover, Cover[agent.Row-1, agent.Col])

    //         if(agent.Row+2<opponent.Row || 
    //         agent.Row+2 == opponent.Row && 2 <= Math.Abs(agent.Col-opponent.Col))
    //     cover = Math.Max(cover, Cover[agent.Row+1, agent.Col])
    //     return cover;

    // }
}