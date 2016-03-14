using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PokeModRed.Items.Weapons;
using PokeModRed.Projectiles.Minions.PokemonProjectiles;

namespace PokeModRed.NPCs
{
	public abstract class PokemonNPC : ModNPC
	{	
		//properties that need to be synchronized
		// once and then on demand
		public byte capture = 0; //current capture status, 0 if not currently undergoing attempted capture
		public float ballRate = 0; // rate of ball attempting capture currently, 0 if not undergoing attempted captured
		
		// once
		public byte nature;
		public byte HPIV;
		public byte AtkIV;
		public byte DefIV;
		public byte SpAIV;
		public byte SpDIV;
		public byte SpeIV;
		
		// once and then on level up
		public byte level;
		public byte HPEV;
		public byte AtkEV;
		public byte DefEV;
		public byte SpAEV;
		public byte SpDEV;
		public byte SpeEV;
		
		// internals
		public PokemonWeapon pokemon; //does this need to be synced?
		private bool set = false;
        public float movSpeed;
        //private bool netUpdate = true;

        //internals that are the same for all Pokemon of certain species or can be derived from synchronized properties
        public virtual float id {get; protected set;}
		public virtual float idleAccel {get{return 0.978f;}}
		public virtual float spacingMult {get{return 1f;}}
		public virtual float viewDist {get{return 400f;}}
		public virtual float chaseDist {get{return 25f;}}
		public virtual float chaseAccel {get{return 6f;}}
		public virtual float inertia {get{return 40f;}}
		public virtual float shootCool {get{return 270f;}}
		public virtual float shootSpeed { get { return 12f; } }
        public virtual int shoot {get{return -1;}}
        public virtual float speed { get { return (float)Spe*2 / (float)level; } }
        public virtual byte aiMode { get { return running; } }
        public int maxHP
		{
			get {
				return ((((2*baseHP)+HPIV+(HPEV/4))*level)/100)+level+10;
			}
		}
		public int Atk
		{
			get {
				return (int)((float)(((((2*baseAtk)+AtkIV+(AtkEV/4))*level)/100)+level+5)*NatureMultipler("Atk"));
			}
		}
		
		public int Def
		{
			get {
				return (int)((float)(((((2*baseDef)+DefIV+(DefEV/4))*level)/100)+level+5)*NatureMultipler("Def"));
			}
		}
		
		public int SpA
		{
			get {
				return (int)((float)(((((2*baseSpA)+SpAIV+(SpAEV/4))*level)/100)+level+5)*NatureMultipler("SpA"));
			}
		}
		
		public int SpD
		{
			get {
				return (int)((float)(((((2*baseSpD)+SpDIV+(SpDEV/4))*level)/100)+level+5)*NatureMultipler("SpD"));
			}
		}
		
		public int Spe
		{
			get {
				return (int)((float)(((((2*baseSpe)+SpeIV+(SpeEV/4))*level)/100)+level+5)*NatureMultipler("Spe"));
			}
		}
		
		// constants
		public const byte Hardy = 1;
		public const byte Lonely = 2;
		public const byte Brave = 3;
		public const byte Adamant = 4;
		public const byte Naughty = 5;
		public const byte Bold = 6;
		public const byte Docile = 7;
		public const byte Relaxed = 8;
		public const byte Impish = 9;
		public const byte Lax = 10;
		public const byte Timid = 11;
		public const byte Hasty = 12;
		public const byte Serious = 13;
		public const byte Jolly = 14;
		public const byte Naive = 15;
		public const byte Modest = 16;
		public const byte Mild = 17;
		public const byte Quiet = 18;
		public const byte Bashful = 19;
		public const byte Rash = 20;
		public const byte Calm = 21;
		public const byte Gentle = 22;
		public const byte Sassy = 23;
		public const byte Careful = 24;
		public const byte Quirky = 25;
		
		public const int erratic = 600000;
		public const int fast = 800000;
		public const int medium_fast = 1000000;
		public const int medium_slow = 1059860;
		public const int slow = 1250000;
		public const int fluctuating = 1640000;
		
		public const byte running = 1;
        public const byte flying = 2;
        public const byte swimming = 3;

        public int catchRate {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.Catch;
				} else {
					return 0;
				}
			}
		}
		
		public int baseHP {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.HP;
				} else {
					return 0;
				}
			}
		}
		public int baseAtk {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.Atk;
				} else {
					return 0;
				}
			}
		}
		public int baseDef {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.Def;
				} else {
					return 0;
				}
			}
		}
		public int baseSpA {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.SpA;
				} else {
					return 0;
				}
			}
		}
		public int baseSpD {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.SpD;
				} else {
					return 0;
				}
			}
		}
		public int baseSpe {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.Spe;
				} else {
					return 0;
				}
			}
		}
		public int EXP {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.EXP;
				} else {
					return 0;
				}
			}
		}
		public int EXPV {
			get{
				PokedexEntry val;
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					return val.EXPV;
				} else {
					return 0;
				}
			}
		}
		public List<KeyValuePair<int, string>> EV_Worth {
			get{
				PokedexEntry val;
				List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
				if (Pokedex.pokedex.TryGetValue(id, out val))
				{
					string str = val.EV_Worth;
					for (int i=0; i< str.Length; i++)
					{
						if (Char.IsDigit(str[i]))
						{
							string stat ="";
							stat += str[i+2];
							stat += str[i+3];
							stat += str[i+4];
							list.Add(new KeyValuePair<int, string>(str[i]-'0', stat));
						}
					}
				}
				return list;
			}
		}
		
		public override void SetDefaults()
		{				
			PokedexEntry val;
			if (Pokedex.pokedex.TryGetValue(id, out val))
			{
				npc.name = val.Pokemon;
			} else {
				npc.name = "";
			}
			Random rnd = new Random();
			nature = (byte)rnd.Next(1,25);
			HPIV = (byte)rnd.Next(0,31);
			AtkIV = (byte)rnd.Next(0,31);
			DefIV = (byte)rnd.Next(0,31);
			SpAIV = (byte)rnd.Next(0,31);
			SpDIV = (byte)rnd.Next(0,31);
			SpeIV = (byte)rnd.Next(0,31);
			HPEV = (byte)rnd.Next(0,31);
			AtkEV = (byte)rnd.Next(0,31);
			DefEV = (byte)rnd.Next(0,31);
			SpAEV = (byte)rnd.Next(0,31);
			SpDEV = (byte)rnd.Next(0,31);
			SpeEV = (byte)rnd.Next(0,31);
			int spawnLevel = 1;
			int spawnFactor = 2;
			if (NPC.downedBoss1){spawnLevel+=spawnFactor;}
			if (NPC.downedBoss2){spawnLevel+=spawnFactor;}
			if (NPC.downedBoss3){spawnLevel+=spawnFactor;}
			if (NPC.downedQueenBee){spawnLevel+=spawnFactor;}
			if (Main.hardMode){spawnLevel+=spawnFactor;}
			if (NPC.downedMechBoss1){spawnLevel+=spawnFactor;}
			if (NPC.downedMechBoss2){spawnLevel+=spawnFactor;}
			if (NPC.downedMechBoss3){spawnLevel+=spawnFactor;}
			if (NPC.downedPlantBoss){spawnLevel+=spawnFactor;}
			if (NPC.downedGolemBoss){spawnLevel+=spawnFactor;}
			if (NPC.downedAncientCultist){spawnLevel+=spawnFactor;}			
			level = (byte)rnd.Next(spawnLevel,spawnLevel+4);
			npc.displayName = npc.name;
			//npc.displayName = "Lvl " +level.ToString() +" " +npc.name; //WORKS BUT MAKES DEBUGGING WITH /npc DIFFICULT AS YOU NEED TO TYPE /npc Lvl # Caterpie TO GET A CATERPIE
			npc.friendly = true;
			npc.damage = Atk;
			npc.defense = Def;
			npc.lifeMax = maxHP;
			npc.life = maxHP;
			npc.knockBackResist = 1.0f;
			Main.npcCatchable[mod.NPCType(npc.name)] = true;
			npc.soundHit = mod.GetSoundSlot(SoundType.NPCHit, "Sounds/NPCHit/NormalDamage");
			npc.soundKilled = mod.GetSoundSlot(SoundType.NPCKilled, "Sounds/NPCKilled/id"+((int)id).ToString());
		}
		
		public override void SendExtraAI(BinaryWriter writer)
		{

		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{

		}
		
		public override void AI()
		{
            if (!set && npc.releaseOwner != 255)
            {
                if (Main.myPlayer == npc.releaseOwner)
                {
                    Main.NewText("Go " + npc.name + "!");
                }
                pokemon = Main.player[npc.releaseOwner].inventory[Main.player[npc.releaseOwner].selectedItem].modItem as PokemonWeapon;
                level = pokemon.level;
                nature = pokemon.nature;
                HPIV = pokemon.HPIV;
                AtkIV = pokemon.AtkIV;
                DefIV = pokemon.DefIV;
                SpAIV = pokemon.SpAIV;
                SpDIV = pokemon.SpDIV;
                SpeIV = pokemon.SpeIV;
                HPEV = pokemon.HPEV;
                AtkEV = pokemon.AtkEV;
                DefEV = pokemon.DefEV;
                SpAEV = pokemon.SpAEV;
                SpDEV = pokemon.SpDEV;
                SpeEV = pokemon.SpeEV;
                npc.damage = Atk;
                npc.defense = Def;
                npc.lifeMax = maxHP;
                npc.life = maxHP;
                Main.player[npc.releaseOwner].AddBuff(mod.BuffType(npc.name+"Buff"), 3600);
                set = true;
            }
			if (!set && npc.releaseOwner == 255)
			{
				npc.friendly = false;
				set = true;
			}

            if (npc.wet)
            {
                if (aiMode == swimming)
                {
                    movSpeed = speed * 2;
                }
                movSpeed = speed / 4;
            }
            else
            {
				if (aiMode == swimming)
                {
                    movSpeed = speed / 4;
                }
                movSpeed = speed;
            }
            
            int pDirection;
			Vector2 pCenter;
			// look at each other active npc and if it is owned by this player then maintain a bit of spacing instead of all clumping up
			if (npc.releaseOwner!=255)
			{
				pCenter = Main.player[npc.releaseOwner].Center;
                if (aiMode == flying)
                {
                    pCenter.Y = pCenter.Y - 60f;
                }
				pDirection = Main.player[npc.releaseOwner].direction;
				npc.friendly = true;
			} else {
                pCenter = new Vector2(0, 0);
                pDirection = 1;
                npc.friendly = false;
            }
            Vector2 targetPos = npc.position;
            float targetDist = viewDist;
            bool target = false;
            npc.noTileCollide = false;
            float spacing = (float)npc.width * spacingMult;
            if (npc.friendly)
			{
                    
				for (int k = 0; k < 200; k++)
				{
                    NPC otherNPC = Main.npc[k];
                    if (k != npc.whoAmI && otherNPC.active && otherNPC.releaseOwner == npc.releaseOwner && System.Math.Abs(npc.position.X - otherNPC.position.X) + System.Math.Abs(npc.position.Y - otherNPC.position.Y) < spacing)
                    {
                        if (npc.position.X < otherNPC.position.X)
                        {
                            npc.velocity.X -= idleAccel;
                        }
                        else
                        {
                            npc.velocity.X += idleAccel;
                        }
                        if (npc.position.Y < otherNPC.position.Y)
                        {
                            if (aiMode == flying)
                            {
                                npc.velocity.Y -= idleAccel;
                            }
                        }
                        else
                        {
                            npc.velocity.Y += idleAccel;
                        }
					}
                    if (otherNPC.CanBeChasedBy(this, false))
                    {
                        float distance = Vector2.Distance(otherNPC.Center, npc.Center);
                        if ((distance < targetDist || !target) && Collision.CanHitLine(npc.position, npc.width, npc.height, otherNPC.position, otherNPC.width, otherNPC.height))
                        {
                            targetDist = distance;
                            targetPos = otherNPC.Center;
                            target = true;
                        }
                    }
                }
			} else {
                // enemy npc ai for targeting

                // don't have a target? then get one
                if (npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active)
                {
                    npc.TargetClosest(true);
                }
                // now check again, if you have one set the location etc.
                if (!(npc.target < 0 || npc.target == 255 || Main.player[npc.target].dead || !Main.player[npc.target].active))
                {
                    targetPos = Main.player[npc.target].position;
                    targetPos.Y = targetPos.Y + (Main.player[npc.target].height / 2);
                    target = true;
                    pCenter = Main.player[npc.target].Center;
                    pDirection = Main.player[npc.target].direction;
                }
            }

            Vector2 myDirection;
            float distanceTo;
            if (npc.friendly)
            {
                myDirection = pCenter - npc.Center;
                distanceTo = myDirection.Length();
                // if the distance is too great, just instant teleport to the player
                if (distanceTo > 1000f)
                {
                    npc.Center = pCenter;
                }
            }
            //if you have a target and you are at ai[0] state 0f, aka ready and not currently fleeing back to npc.releaseOwner
            if (target && npc.ai[0] == 0f)
			{
				// get the distance between this and it's target
				myDirection = targetPos - npc.Center;
				// if the distance is greater than 'chaseDist', the distance this engages enemies from
				if (myDirection.Length() > chaseDist)
				{
					// then move toward the target
					myDirection.Normalize();
					npc.velocity = (npc.velocity * inertia + myDirection * chaseAccel) / (inertia + 1);
				}
				else
				{
					//otherwise slowly come to a halt
					npc.velocity *= (float)Math.Pow(0.97, 40.0 / inertia);
				}
            }
			else
			{
                if (npc.friendly)
                {
                    int num = 1;
                    myDirection = pCenter - npc.Center;
                    distanceTo = myDirection.Length();
                    npc.ai[1] = 3600f;
                    npc.netUpdate = true;
                    if (distanceTo > 48f)
					{
						myDirection.Normalize();
						myDirection *= movSpeed;
						float temp = inertia / 2f;
						npc.velocity = (npc.velocity * temp + myDirection) / (temp + 1);
					}
					else
					{
						npc.direction = pDirection;
						npc.velocity *= (float)Math.Pow(0.9, 40.0 / inertia);
                        if (aiMode == flying) //************************* FLYING *************************
                        {
                            if (npc.velocity.X == 0f && npc.velocity.Y == 0f)
                            {
                                npc.velocity.X = -0.15f;
                                npc.velocity.Y = -0.05f;
                            }
                            npc.velocity *= 1.01f;
                        }
					}
                }
            }
            

            npc.rotation = npc.velocity.X * 0.05f;
			if (npc.velocity.X > 0f)
			{
				npc.spriteDirection = (npc.direction = 1);
			}
			else if (npc.velocity.X < 0f)
			{
				npc.spriteDirection = (npc.direction = -1);
			}
			if (npc.ai[1] > 0f)
			{
				npc.ai[1] += 1f;
				if (Main.rand.Next(3) == 0)
				{
					npc.ai[1] += 1f;
				}
			}
			if (npc.ai[1] > shootCool)
			{
				npc.ai[1] = 0f;
				npc.netUpdate = true;
			}
			if (npc.ai[0] == 0f)
			{
				if (target)
				{
					if ((targetPos - npc.Center).X > 0f)
					{
						npc.spriteDirection = (npc.direction = 1);
					}
					else if ((targetPos - npc.Center).X < 0f)
					{
						npc.spriteDirection = (npc.direction = -1);
					}
					if (npc.ai[1] == 0f)
					{
						npc.ai[1] = 1f;
                        if (Main.netMode != 1)
                        {
                            // shoot code
                            if (shoot != -1)
							{
								Vector2 shootVel = targetPos - npc.Center;
								if (shootVel == Vector2.Zero)
								{
									shootVel = new Vector2(0f, 1f);
								}
								shootVel.Normalize();
								shootVel *= shootSpeed;
                                int owner;
                                if (npc.friendly)
                                {
                                    owner = Main.player[npc.releaseOwner].whoAmI;
                                } else
                                {
                                    owner = 255;
                                }
								int proj = Projectile.NewProjectile(npc.Center.X, npc.position.Y, shootVel.X, shootVel.Y, shoot, GetRangedDamage(), GetKnockback(), owner, 0f, 0f);
								PokemonProjectile pokemonProjectile;
								pokemonProjectile = Main.projectile[proj].modProjectile as PokemonProjectile;
								pokemonProjectile.pokemon = this.pokemon;
								Main.projectile[proj].timeLeft = 300;
								Main.projectile[proj].netUpdate = true;
                                if (owner == 255)
                                {
                                    Main.projectile[proj].friendly = false;
                                    Main.projectile[proj].hostile = true;
                                }
                                else {
                                    Main.projectile[proj].friendly = true;
                                    Main.projectile[proj].hostile = false;
                                }
                                npc.netUpdate = true;
							}
						}
					}
				}
			}
            // JUMP GAPS
            if (aiMode == running)
            {
                bool flag5 = true;
                if (npc.velocity.X == 0f)
                {
                    flag5 = false;
                }
                if (npc.justHit)
                {
                    flag5 = false;
                }
                int num210 = (int)((npc.position.X + (float)(npc.width / 2) + (float)(15 * npc.direction)) / 16f);
                int num211 = (int)((npc.position.Y + (float)npc.height - 15f) / 16f);
                //num210 = (int)((npc.position.X + (float)(npc.width / 2) + (float)((npc.width / 2 + 16) * npc.direction)) / 16f);
                if (Main.tile[num210, num211] == null)
                {
                    Main.tile[num210, num211] = new Tile();
                }
                if (Main.tile[num210, num211 - 1] == null)
                {
                    Main.tile[num210, num211 - 1] = new Tile();
                }
                if (Main.tile[num210, num211 - 2] == null)
                {
                    Main.tile[num210, num211 - 2] = new Tile();
                }
                if (Main.tile[num210, num211 - 3] == null)
                {
                    Main.tile[num210, num211 - 3] = new Tile();
                }
                if (Main.tile[num210, num211 + 1] == null)
                {
                    Main.tile[num210, num211 + 1] = new Tile();
                }
                if (Main.tile[num210 + npc.direction, num211 - 1] == null)
                {
                    Main.tile[num210 + npc.direction, num211 - 1] = new Tile();
                }
                if (Main.tile[num210 + npc.direction, num211 + 1] == null)
                {
                    Main.tile[num210 + npc.direction, num211 + 1] = new Tile();
                }
                if (Main.tile[num210 - npc.direction, num211 + 1] == null)
                {
                    Main.tile[num210 - npc.direction, num211 + 1] = new Tile();
                }
                Main.tile[num210, num211 + 1].halfBrick();
                if (Main.tile[num210, num211 - 1].nactive() && (Main.tile[num210, num211 - 1].type == 10 || Main.tile[num210, num211 - 1].type == 388))
                {
                    npc.ai[2] += 1f;
                    npc.ai[3] = 0f;
                    if (npc.ai[2] >= 60f)
                    {
                        npc.velocity.X = 0.5f * (float)(-(float)npc.direction);
                        int num212 = 5;
                        if (Main.tile[num210, num211 - 1].type == 388)
                        {
                            num212 = 2;
                        }
                        npc.ai[1] += (float)num212;
                        npc.ai[2] = 0f;
                    }
                }
                else
                {
                    int num213 = npc.spriteDirection;
                    if ((npc.velocity.X < 0f && num213 == -1) || (npc.velocity.X > 0f && num213 == 1))
                    {
                        if (npc.height >= 32 && Main.tile[num210, num211 - 2].nactive() && Main.tileSolid[(int)Main.tile[num210, num211 - 2].type])
                        {
                            if (Main.tile[num210, num211 - 3].nactive() && Main.tileSolid[(int)Main.tile[num210, num211 - 3].type])
                            {
                                npc.velocity.Y = -8f;
                                npc.netUpdate = true;
                            }
                            else
                            {
                                npc.velocity.Y = -7f;
                                npc.netUpdate = true;
                            }
                        }
                        else if (Main.tile[num210, num211 - 1].nactive() && Main.tileSolid[(int)Main.tile[num210, num211 - 1].type])
                        {
                            npc.velocity.Y = -6f;
                            npc.netUpdate = true;
                        }
                        else if (npc.position.Y + (float)npc.height - (float)(num211 * 16) > 20f && Main.tile[num210, num211].nactive() && !Main.tile[num210, num211].topSlope() && Main.tileSolid[(int)Main.tile[num210, num211].type])
                        {
                            npc.velocity.Y = -5f;
                            npc.netUpdate = true;
                        }
                        else if (npc.directionY < 0 && npc.type != 67 && (!Main.tile[num210, num211 + 1].nactive() || !Main.tileSolid[(int)Main.tile[num210, num211 + 1].type]) && (!Main.tile[num210 + npc.direction, num211 + 1].nactive() || !Main.tileSolid[(int)Main.tile[num210 + npc.direction, num211 + 1].type]))
                        {
                            npc.velocity.Y = -8f;
                            npc.velocity.X = npc.velocity.X * 1.5f;
                            npc.netUpdate = true;
                        }
                    }
                    if (npc.velocity.Y == 0f && Math.Abs(npc.position.X + (float)(npc.width / 2) - (Main.player[npc.target].position.X + (float)(Main.player[npc.target].width / 2))) < 100f && Math.Abs(npc.position.Y + (float)(npc.height / 2) - (Main.player[npc.target].position.Y + (float)(Main.player[npc.target].height / 2))) < 50f && ((npc.direction > 0 && npc.velocity.X >= 1f) || (npc.direction < 0 && npc.velocity.X <= -1f)))
                    {
                        npc.velocity.X = npc.velocity.X * 2f;
                        if (npc.velocity.X > 3f)
                        {
                            npc.velocity.X = 3f;
                        }
                        if (npc.velocity.X < -3f)
                        {
                            npc.velocity.X = -3f;
                        }
                        npc.velocity.Y = -4f;
                        npc.netUpdate = true;
                    }
                }
            }
            if (capture > 0)
            {
                Capture();
            }
            CreateDust();
        }
		
        public void Capture()
        {
            // see http://bulbapedia.bulbagarden.net/wiki/Catch_rate for forumla
            float bonusStatus = 1.0f;
            int a = (int)((((3f * (float)npc.lifeMax - (2f * (float)npc.life)) * (float)catchRate * ballRate) / (3f * (float)npc.lifeMax)) * bonusStatus);
            if (a > 255 || ballRate == 255) { a = 255; }
            int b = (int)((double)1048560 / Math.Sqrt(Math.Sqrt((double)16711680 / (double)a)));
            Random rnd = new Random();
            for (int i=0; i < 3; i++)
            {
                if (rnd.Next(0, 65535) >= b)
                {
                    if (i==0)
                    {
                        Main.NewText("Oh no! The Pokémon broke free!");
                        capture = 0;
                        return;
                    } else if (i==1)
                    {
                        Main.NewText("Aww! It appeared to be caught!");
                        capture = 0;
                        return;
                    } else if (i==2)
                    {
                        Main.NewText("Aargh! Almost had it!");
                        capture = 0;
                        return;
                    } else if (i == 3)
                    {
                        Main.NewText("Gah! It was so close, too!");
                        capture = 0;
                        return;
                    }
                }
            }
            Main.NewText("Gotcha! " +npc.name +" was caught!");
            int itemRef = Item.NewItem((int)npc.position.X, (int)npc.position.Y, 1, 1, mod.ItemType(npc.name+"Pokeball"), 1, false, 0, false, false);
            PokemonWeapon newItem;
            newItem = Main.item[itemRef].modItem as PokemonWeapon;
            newItem.level = this.level;
            newItem.nature = this.nature;
            newItem.experience = 0;
            newItem.HPIV = this.HPIV;
            newItem.HPEV = this.HPEV;
            newItem.AtkIV = this.AtkIV;
            newItem.AtkEV = this.AtkEV;
            newItem.DefIV = this.DefIV;
            newItem.DefEV = this.DefEV;
            newItem.SpAIV = this.SpAIV;
            newItem.SpAEV = this.SpAEV;
            newItem.SpDIV = this.SpDIV;
            newItem.SpDEV = this.SpDEV;
            newItem.SpeIV = this.SpeIV;
            newItem.SpeEV = this.SpeEV;
            newItem.SetToolTip();
            capture = 0;
            npc.active = false;
        }

		public override bool CheckDead()
		{
			if (set && npc.releaseOwner != 255 && Main.player[npc.releaseOwner].HasBuff(mod.BuffType(npc.name+"Buff")) > -1)
			{
				if (Main.myPlayer == npc.releaseOwner)
				{
					Main.NewText(npc.name +" has fainted!");
				}
				Main.player[npc.releaseOwner].DelBuff(Main.player[npc.releaseOwner].HasBuff(mod.BuffType(npc.name+"Buff")));
			}
			return true;
		}
		
		public override bool CheckActive()
		{
			if (set && npc.active && npc.releaseOwner != 255 && Main.player[npc.releaseOwner].HasBuff(mod.BuffType(npc.name+"Buff")) < 0)
			{
				if (Main.myPlayer == npc.releaseOwner)
				{
					Main.NewText(npc.name +", ok! Come back!");
				}
				npc.life = 0;
				npc.active = false;
				npc.netUpdate = true;
			}
			if (npc.friendly)
			{
				return false; //doesn't count against max npc limit near players (as they are player summoned and shouldn't reduce total enemy npcs)
			}
			return true;
		}
		
		public override void FindFrame(int frameHeight)
		{
			if (Math.Abs(npc.velocity.X) > 1.0f)
			{
				npc.frameCounter+=1f;
				npc.frame.Y = frameHeight * ((int)(npc.frameCounter/7) % Main.npcFrameCount[npc.type]);
			} else {
				npc.frameCounter = 0;
				npc.frame.Y = 0;
			}
			
		}
		
		public virtual void CreateDust()
		{
		}
		
		public int GetRangedDamage()
		{
			return SpA;
		}
		
		public float GetKnockback()
		{
			return 1.0f;
		}
		
		public float NatureMultipler(string stat)
		{
			if (nature == 1 || nature == 7 || nature == 13 || nature == 19 || nature == 25){
				return 1f;
			} else if (stat == "Atk") {
				if (nature == 2 || nature == 3 || nature == 4 || nature == 5) {return 1.10f;}
				else if (nature == 6 || nature == 11 || nature == 16 || nature == 21) {return 0.9f;}
			} else if (stat == "Def") {
				if (nature == 6 || nature == 8 || nature == 9 || nature == 10) {return 1.10f;}
				else if (nature == 2 || nature == 12 || nature == 17 || nature == 22) {return 0.9f;}
			} else if (stat == "SpA") {
				if (nature == 16 || nature == 17 || nature == 18 || nature == 20) {return 1.10f;}
				else if (nature == 4 || nature == 9 || nature == 14 || nature == 24) {return 0.9f;}
			} else if (stat == "SpD") {
				if (nature == 21 || nature == 22 || nature == 23 || nature == 24) {return 1.10f;}
				else if (nature == 5 || nature == 10 || nature == 15 || nature == 20) {return 0.9f;}
			} else if (stat == "Spe") {
				if (nature == 11 || nature == 12 || nature == 14 || nature == 15) {return 1.10f;}
				else if (nature == 3 || nature == 8 || nature == 18 || nature == 23) {return 0.9f;}
			}
			return 1f;
		}
		
		public int GetExpForLevel(int level)
		{
			if (EXP == erratic)
			{
				if (level <= 50)
				{
					return (((level * level * level) * (100 - level)) / 50);
				} else if (level > 50 && level <= 68)
				{
					return (((level * level * level) * (150 - level)) / 100);
				} else if (level > 68 && level <= 98) 
				{
					return (((level * level * level) * (1911 - (10 * level))) / 3);
				} else if (level > 98 && level <= 100)
				{
					return (((level * level * level) * (160 - level)) / 100);
				}
			} else if (EXP == fast)
			{
				return (  ( 4 * ( level * level * level) ) / 5 );
			} else if (EXP == medium_fast)
			{
				return ( level * level * level);
			} else if (EXP == medium_slow)
			{
				return (((6 / 5)*(level * level * level)) - (15 * (level * level)) + (100*level) - 140);
			} else if (EXP == slow)
			{
				return ((5 * (level * level * level)) / 4);
			} else if (EXP == fluctuating)
			{
				if (level <= 15)
				{
					return ((level * level * level) * ((((level+1)/3)+24)/50));
				} else if (level > 15 && level <= 36)
				{
					return ((level * level * level) * ((level+14)/50));
				} else if (level > 36 && level <= 100) 
				{
					return ((level * level * level) * (((level/2)+32)/50));
				}
			}
			return 999999999;
		}
		
		public string GetNatureString()
		{
			switch (nature)
			{
				case 1:
					return "Hardy";
				case 2:
					return "Lonely";
				case 3:
					return "Brave";
				case 4:
					return "Adamant";
				case 5:
					return "Naughty";
				case 6:
					return "Bold";
				case 7:
					return "Docile";
				case 8:
					return "Relaxed";
				case 9:
					return "Impish";
				case 10:
					return "Lax";
				case 11:
					return "Timid";
				case 12:
					return "Hasty";
				case 13:
					return "Serious";
				case 14:
					return "Jolly";
				case 15:
					return "Naive";
				case 16:
					return "Modest";
				case 17:
					return "Mild";
				case 18:
					return "Quiet";
				case 19:
					return "Bashful";
				case 20:
					return "Rash";
				case 21:
					return "Calm";
				case 22:
					return "Gentle";
				case 23:
					return "Sassy";
				case 24:
					return "Careful";
				case 25:
					return "Quirky";
				default:
					return "Hardy";
			}
		}
		
		public string StatLine()
		{
			return "HP: " +maxHP.ToString() +", IV: " +HPIV.ToString() +", EV: " +HPEV.ToString() + " Attack: " +Atk.ToString() +", IV: " +AtkIV.ToString() +", EV: " +AtkEV.ToString() +" Defense: " +Def.ToString() +", IV: " +DefIV.ToString() +", EV: " +DefEV.ToString() +System.Environment.NewLine +"Special Attack: " +SpA.ToString() +", IV: " +SpAIV.ToString() +", EV: " +SpAEV.ToString() + " Special Defense: " +SpD.ToString() +", IV: " +SpDIV.ToString() +", EV: " +SpDEV.ToString() + " Speed: " +Spe.ToString() +", IV: " +SpeIV.ToString() +", EV: " +SpeEV.ToString();
		}
		
		// Acts as a multiplier to reduce or increase all Pokemon spawns
		public override float CanSpawn(NPCSpawnInfo spawnInfo)
		{
			if (catchRate > 235)
			{
				return ((float)catchRate/255f)/16f;
			} else if (catchRate <= 235 && catchRate >= 190)
			{
				if (NPC.downedBoss1)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 190 && catchRate >= 150)
			{
				if (NPC.downedBoss2)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 150 && catchRate >= 100)
			{
				if (NPC.downedBoss3)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 100 && catchRate >= 80)
			{
				if (Main.hardMode)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 80 && catchRate >= 60)
			{
				if (NPC.downedMechBoss1)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 60 && catchRate >= 40)
			{
				if (NPC.downedMechBoss2)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 40 && catchRate >= 30)
			{
				if (NPC.downedMechBoss3)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 30 && catchRate >= 20)
			{
				if (NPC.downedPlantBoss)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else if (catchRate < 20 && catchRate >= 10)
			{
				if (NPC.downedAncientCultist)
				{
					return ((float)catchRate/255f)/16f;
				} else {
					return 0f;
				}
			} else {
				return 0f;
			}
		}
	}
}