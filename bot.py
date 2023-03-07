import discord
from discord.ext import commands
 
bot = commands.Bot(command_prefix='.', intents=discord.Intents.all())
 
@bot.event
async def on_ready():
    print(f'Login bot: {bot.user}')
 
@bot.command()
async def hello(message):
    await message.channel.send('Hi!')

@bot.command()
async def 페이백(ctx, tax, arg1, arg2):
    result = (100-int(tax)) / 100
    print(result)
    payback = (int(arg1) * result) - (int(arg2) * result)
    print(payback)
    await ctx.channel.send(payback)
 
bot.run('MTA4MjMzNzcyMjI2MzQyMTA4MA.G9fCA4.SLF_DSexoIJlkqe86OdQ5XVgJr_MMxsCkrkCfI')