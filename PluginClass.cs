using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fCraft;
using fCraft.Events;

namespace SpawnPhysics {
    public class Init : Plugin {
        //Plugin Initialize
        public void Initialize() {
            Logger.Log( LogType.ConsoleOutput, "Loading SpawnPhysics " + Version );
            Player.JoinedWorld += Player_JoiningWorld;
        }

        public string Name {
            get {
                return "SpawnPhysics";
            }
            set {
                Name = value;
            }
        }

        public string Version {
            get {
                return "1.1";
            }
            set {
                Version = value;
            }
        }

        /// <summary>
        /// Event which occurs when a player is joining a world
        /// </summary>
        /// <param name="e">event params</param>
        public static void Player_JoiningWorld( object sender, PlayerJoinedWorldEventArgs e ) {
            World world = e.NewWorld;
            Player player = e.Player;
            if ( world != null ) { //run anti null checks
                if ( world.IsLoaded ) {
                    if ( world.Map != null ) {
                        //Change the spawn position to block coords (X / 32, Y / 32, Z / 32)
                        Vector3I Pos = player.Position.ToBlockCoords();
                        bool foundPos = false;
                        //iterate from the maps height, to Zero. If a block is not equal to air, change the spawn point
                        for ( int z = Pos.Z; z > 0; z-- ) {
                            if ( world.Map.GetBlock( Pos.X, Pos.Y, z ) != Block.Air &&
                                world.Map.GetBlock( Pos.X, Pos.Y, z ) != Block.Undefined ) {
                                //Z + 3 so the player's feet are above the block pos
                                Position toSend = new Vector3I( Pos.X, Pos.Y, z + 3 ).ToPlayerCoords();
                                player.TeleportTo( toSend ); //teleport the player
                                foundPos = true;
                                break;
                            }
                        }
                        if ( !foundPos ) { //presume the map is empty and set the zPos to 0
                            Position toSend = new Vector3I( Pos.X, Pos.Y, 3 ).ToPlayerCoords();
                            player.TeleportTo( toSend ); //teleport the player
                        }
                    }
                }
            }
        }
    }
}