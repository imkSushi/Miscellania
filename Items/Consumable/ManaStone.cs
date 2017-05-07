﻿
using System;
using System.Collections.Generic;
using System.IO;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace GoldensMisc.Items.Consumable
{
	public class ManaStone : ModItem
	{
		public override bool Autoload(ref string name, ref string texture, IList<EquipType> equips)
		{
			return Config.MagicStones;
		}
		
		byte uses = 0;
		const byte maxUses = 150;
		
		public override void SetDefaults()
		{
			item.name = "Mana Stone";
			item.width = 26;
			item.height = 26;
			item.healMana = 150;
			item.useStyle = 4;
			item.useTime = 30;
			item.useAnimation = 30;
			item.UseSound = SoundID.Item29;
			item.consumable = true;
			AddTooltip("Can be used multiple times");
			item.rare = 4;
			item.value = Item.sellPrice(0, 8);
		}
		
		public override bool ConsumeItem(Player player)
		{
			return false;
		}
		
		public override bool UseItem(Player player)
		{
			uses++;
			if(uses >= maxUses)
			{
				item.SetDefaults(mod.ItemType<InertStone>());
			}
			return true;
		}
		
		public override void HoldStyle(Player player)
		{
			player.itemLocation.X -= 10 * player.direction;
			player.itemLocation.Y += 10 * player.gravDir;
		}
		
		public override void AddRecipes()
		{
			var recipe = new ModRecipe(mod);
			recipe.AddIngredient(mod.ItemType<InertStone>());
			recipe.AddIngredient(ItemID.ManaCrystal, 3);
			recipe.AddIngredient(ItemID.CrystalShard, 5);
			recipe.AddTile(TileID.CrystalBall);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		
		public override bool CloneNewInstances
		{
			get { return true; }
		}
		
		public override void NetSend(BinaryWriter writer)
		{
			writer.Write(uses);
		}
		
		public override void NetRecieve(BinaryReader reader)
		{
			uses = reader.ReadByte();
		}
		
		public override TagCompound Save()
		{
			return new TagCompound
			{
				{"u", uses}
			};
		}
		
		public override void Load(TagCompound tag)
		{
			uses = tag.GetByte("u");
		}
	}
}
