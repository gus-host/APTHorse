# APTHorse

**Multiplayer On-chain Horse Racing game powered by Aptos Random Roll Mint your destined APThorse powered by random acceleration, velocity & hurdles and challenge your friends and top the leaderboards**
<img width="1192" alt="Screenshot 2024-03-23 at 12 07 00 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/8182b0b9-1188-4a19-91ae-71d6632c2caa">




**Check out our smart contracts here:**
[https://github.com/IntoTheVerse/APTHorse-contracts](https://github.com/IntoTheVerse/APTHorse-contracts)

### Built With
**Built using**
- Aptos Unity SDK (official licensed): https://github.com/aptos-labs/Aptos-Unity-SDK the sDK creates HD wallet for every user who signs up
- Move Language
- Aptos Randomness (Aptos Random Roll on-chain API)
- Unity and Photon Engine
- Hasura Graph QL indexer (to index transaction data from Aptos Randomet to the client)

#### The game is completely simulated on the blockchain, so that in future, the developers can port unique clients to the Aptos smart contract backend, and play their races. We only use Photon engine to set up rooms for every raceID fetched from the Aptos Network

# Features built
✅ On-chain player profile creation, race creation, start race, end race and staking APT tokens to participate and withdrawal upon leaving race and race creation rooms 
✅ In-game on-chain randomness, random max velocity - assigned upon mint, random acceleration and random position of puddles on racetrack assigned upon starting a race 
✅ In-game marketplace to mint your random horse which determines their maximum speed
✅ On-chain gameplay - reading velocity, acceleration and puddle position data from blockchain and simulating on the racetrack on the client 
✅ Prize distribution - based on leaderboard powered by randomness within a range of percentage prize pool reserved
✅ Indexing data using C# on unity using graphQL to fetch speeds, acceleration and rewards

<img width="1301" alt="image" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/f523df39-1636-461e-9915-336a8dcdf635">
<img width="1195" alt="Screenshot 2024-03-23 at 12 07 12 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/ea11a201-a2d5-4573-b278-dd1fa9ef3490">
<img width="1198" alt="Screenshot 2024-03-23 at 12 07 40 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/6c3fe226-7bc8-4c53-bd87-ac085e4b0a1f">
<img width="1188" alt="Screenshot 2024-03-23 at 12 07 58 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/7e94e1a0-7c8f-4780-914c-0cfd737a1f80">

## Players mint their destined ATPHorse on Aptos network 

- The randomness determines the maximum velocity for the horse, which is unique and provably fair for every mint powered by Aptos on-chain randomness
- Players join the rooms inside the unity game, and the races are created - deposit 10 APT for 1 lap, 7 APT for 3 Laps, and 4 APT coins for 5 Lap races, they can also leave race by withdrawing their stake
- The race outcome is determined by the horses maximum velocity, and also controlled by random acceleration that is assigned by Aptos randomness at the start of every race, and also position of puddles on the race track, that is also determined by Aptos randomness
- The puddles (marshy tile pieces on the race track)  set player's acceleration to 0 each time the horses encounter them on the track. Acceleration is the rate at which the horse attains its maximum velocity
- Whether, you have the fastest horse, or the slowest, the best acceleration or the least, or whether you encounter the puddle early on in the race or just before the finish track every horse (fast or slow) has an equal stake in the race
- The prize distribution for every race is also determined by the randomness on-chain, which makes sure that the tokens from the initial stake are randomly distributed according to the poisitions in the descending order and as a developer, we retain the smallest share after the random distribution 
- Currently we allow rooms for 5 horses in a multiplayer jockey gaming

### Transactions
[https://explorer.aptoslabs.com/account/0xf5ba4eeade1e3505128e8e7ed36cb147aa4c1fb53ce5a11074ec32dd9f40195c/modules/code/aptos_horses_game/on_race_end?network=randomnet](https://explorer.aptoslabs.com/account/0xf5ba4eeade1e3505128e8e7ed36cb147aa4c1fb53ce5a11074ec32dd9f40195c/modules/code/aptos_horses_game/on_race_end?network=randomnet)

<img width="1046" alt="Screenshot 2024-03-23 at 12 02 01 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/a243e2be-d0bf-410c-814d-4cb456d6019d">
<img width="1026" alt="Screenshot 2024-03-23 at 12 00 00 PM" src="https://github.com/IntoTheVerse/APTHorse/assets/43913734/39a449d0-1fc9-48a8-9a86-dd8dfe71f69e">

### Smart contracts

[https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/horses.move](https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/horses.move)    

- Has all functions related to:

- Collection Aptos NFT standard with attributes like name, price, description, token URI, price and max speed which is determined by Aptos Roll upon mint
-Getting speed of horse upon mint into the unity game engine
- Standard minting functions on Aptos

[https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/publisher_signer.move](https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/publisher_signer.move)    

Has all functions related to:
- Publisher/signer authority

[https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/user.move](https://github.com/IntoTheVerse/APTHorse-contracts/blob/main/sources/user.move)

Has all functions related to:
- Creating user
- Changing username
- Getting username










