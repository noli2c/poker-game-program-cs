using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerGame
{
    enum Suit
    {
        Hearts,
        Diamonds,
        Clubs,
        Spades,
        Joker
    }

    enum Rank
    {
        Ace = 1,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Joker
    }

    struct Card
    {
        public Suit Suit;
        public Rank Rank;
    }

    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                PlayPoker();
                Console.WriteLine("もう一度プレイしますか？ (y/n)");
                string answer = Console.ReadLine();
                if (answer.ToLower() != "y")
                {
                    break;
                }
                Console.Clear();
            }
        }

        static void PlayPoker()
        {
            // デッキの作成
            List<Card> deck = CreateDeck();

            // デッキのシャッフル
            ShuffleDeck(deck);

            // プレイヤーとCPUの手札を配布
            List<Card> playerHand = DealHand(deck, 5);
            List<Card> cpuHand = DealHand(deck, 5);

            // プレイヤーの手札の表示
            Console.WriteLine("プレイヤーの手札:");
            DisplayHand(playerHand);

            // 手札の交換
            Console.WriteLine("手札を交換しますか？ (y/n)");
            string answer = Console.ReadLine();
            if (answer.ToLower() == "y")
            {
                Console.WriteLine("交換するカードの番号を入力してください (1-5、複数選択可、スペース区切り)");
                string input = Console.ReadLine();
                string[] selectedCards = input.Split(' ');

                foreach (string cardIndex in selectedCards)
                {
                    int index = int.Parse(cardIndex) - 1;
                    if (index >= 0 && index < playerHand.Count)
                    {
                        playerHand[index] = DrawCard(deck);
                    }
                }

                // プレイヤーの交換後の手札の表示
                Console.WriteLine("プレイヤーの交換後の手札:");
                DisplayHand(playerHand);
            }

            // CPUの手札の表示
            Console.WriteLine("CPUの手札:");
            DisplayHand(cpuHand);

            // 役の判定と勝敗の表示
            HandRank playerRank = GetHandRank(playerHand);
            HandRank cpuRank = GetHandRank(cpuHand);

            Console.WriteLine("プレイヤーの役: " + playerRank);
            Console.WriteLine("CPUの役: " + cpuRank);

            if (playerRank > cpuRank)
            {
                Console.WriteLine("プレイヤーの勝利！");
            }
            else if (playerRank < cpuRank)
            {
                Console.WriteLine("CPUの勝利！");
            }
            else
            {
                Console.WriteLine("引き分け！");
            }

            Console.WriteLine();
        }

        static List<Card> CreateDeck()
        {
            List<Card> deck = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                if (suit == Suit.Joker)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Card jokerCard = new Card { Suit = Suit.Joker, Rank = Rank.Joker };
                        deck.Add(jokerCard);
                    }
                }
                else
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        if (rank != Rank.Joker)
                        {
                            Card card = new Card { Suit = suit, Rank = rank };
                            deck.Add(card);
                        }
                    }
                }
            }

            return deck;
        }

        static void ShuffleDeck(List<Card> deck)
        {
            Random rng = new Random();

            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = deck[k];
                deck[k] = deck[n];
                deck[n] = value;
            }
        }

        static List<Card> DealHand(List<Card> deck, int numCards)
        {
            List<Card> hand = new List<Card>();

            for (int i = 0; i < numCards; i++)
            {
                Card card = DrawCard(deck);
                hand.Add(card);
            }

            return hand;
        }

        static Card DrawCard(List<Card> deck)
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        static void DisplayHand(List<Card> hand)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                Card card = hand[i];
                Console.WriteLine((i + 1) + ": " + GetCardString(card));
            }
        }

        static string GetCardString(Card card)
        {
            string rankString;
            switch (card.Rank)
            {
                case Rank.Ace:
                    rankString = "A";
                    break;
                case Rank.Jack:
                    rankString = "J";
                    break;
                case Rank.Queen:
                    rankString = "Q";
                    break;
                case Rank.King:
                    rankString = "K";
                    break;
                case Rank.Joker:
                    rankString = "";
                    break;
                default:
                    rankString = ((int)card.Rank).ToString();
                    break;
            }

            string suitString;
            switch (card.Suit)
            {
                case Suit.Hearts:
                    suitString = "♥";
                    break;
                case Suit.Diamonds:
                    suitString = "♦";
                    break;
                case Suit.Clubs:
                    suitString = "♣";
                    break;
                case Suit.Spades:
                    suitString = "♠";
                    break;
                case Suit.Joker:
                    suitString = "Joker";
                    break;
                default:
                    suitString = "";
                    break;
            }

            return suitString + rankString;
        }

static HandRank GetHandRank(List<Card> hand)
{
    // ワイルドカードの枚数を数える
    int wildCardCount = hand.Count(card => card.Rank == Rank.Joker);

    if (HasRoyalFlush(hand, wildCardCount))
    {
        return HandRank.RoyalFlush;
    }
    else if (HasStraightFlush(hand, wildCardCount))
    {
        return HandRank.StraightFlush;
    }
    else if (HasFourOfAKind(hand, wildCardCount))
    {
        return HandRank.FourOfAKind;
    }
    else if (HasFullHouse(hand, wildCardCount))
    {
        return HandRank.FullHouse;
    }
    else if (HasFlush(hand, wildCardCount))
    {
        return HandRank.Flush;
    }
    else if (HasStraight(hand, wildCardCount))
    {
        return HandRank.Straight;
    }
    else if (HasThreeOfAKind(hand, wildCardCount))
    {
        return HandRank.ThreeOfAKind;
    }
    else if (HasTwoPair(hand, wildCardCount))
    {
        return HandRank.TwoPair;
    }
    else if (HasOnePair(hand, wildCardCount))
    {
        return HandRank.OnePair;
    }
    else
    {
        return HandRank.HighCard;
    }
}

static bool HasTwoPair(List<Card> hand, int wildCardCount)
{
    // ランクごとにカードをグループ化
    var rankGroups = hand.GroupBy(card => card.Rank);

    // ペアの数を数える
    int pairCount = 0;

    foreach (var group in rankGroups)
    {
        if (group.Count() >= 2)
        {
            pairCount++;
        }
    }

    // ワイルドカードを使用してツーペアの役が存在するかどうかを判定
    return pairCount >= 2 || (pairCount == 1 && wildCardCount == 1);
}

static bool HasOnePair(List<Card> hand, int wildCardCount)
{
    // ランクごとにカードをグループ化
    var rankGroups = hand.GroupBy(card => card.Rank);

    // ワイルドカードを使用してペアの役が存在するかどうかを判定
    foreach (var group in rankGroups)
    {
        if (group.Count() + wildCardCount >= 2)
        {
            return true;
        }
    }

    return false;
}


static bool HasRoyalFlush(List<Card> hand, int wildCardCount)
{
    // カードをスートごとにグループ化
    var suitGroups = hand.GroupBy(card => card.Suit);

    // スートごとにランクの配列を取得
    var rankArrays = suitGroups.Select(group => group.Select(card => card.Rank).ToArray()).ToList();

    // ロイヤルフラッシュの役を構成するランク
    Rank[] royalFlushRanks = { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace };

    // ワイルドカードを使用してロイヤルフラッシュの役が存在するかどうかを判定
    foreach (var ranks in rankArrays)
    {
        if (ranks.Length + wildCardCount >= 5 && royalFlushRanks.All(rank => ranks.Contains(rank) || wildCardCount > 0))
        {
            return true;
        }
    }

    return false;
}

static bool HasStraightFlush(List<Card> hand, int wildCardCount)
{
    // カードをスートごとにグループ化
    var suitGroups = hand.GroupBy(card => card.Suit);

    // スートごとにランクの配列を取得
    var rankArrays = suitGroups.Select(group => group.Select(card => card.Rank).ToArray()).ToList();

    // ワイルドカードを使用してストレートフラッシュの役が存在するかどうかを判定
    foreach (var ranks in rankArrays)
    {
        if (ranks.Length + wildCardCount >= 5 && IsStraight(ranks, wildCardCount))
        {
            return true;
        }
    }

    return false;
}

static bool IsStraight(Rank[] ranks, int wildCardCount)
{
    Array.Sort(ranks);

    for (int i = 0; i < ranks.Length - 1; i++)
    {
        if (ranks[i] + 1 != ranks[i + 1])
        {
            int gap = ranks[i + 1] - ranks[i] - 1;
            if (gap > wildCardCount)
            {
                return false;
            }
            else
            {
                wildCardCount -= gap;
            }
        }
    }

    return true;
}

static bool HasFlush(List<Card> hand, int wildCardCount)
{
    // カードをスートごとにグループ化
    var suitGroups = hand.GroupBy(card => card.Suit);

    // ワイルドカードを使用してフラッシュの役が存在するかどうかを判定
    foreach (var group in suitGroups)
    {
        int groupCount = group.Count();
        if (groupCount >= 5 || (groupCount + wildCardCount >= 5))
        {
            return true;
        }
    }

    return false;
}


static bool HasStraight(List<Card> hand, int wildCardCount)
{
    var ranks = hand.Select(card => card.Rank).Distinct().ToList();
    ranks.Sort();

    for (int i = 0; i < ranks.Count - 1; i++)
    {
        if (ranks[i] + 1 != ranks[i + 1])
        {
            int gap = ranks[i + 1] - ranks[i] - 1;
            if (gap > wildCardCount)
            {
                return false;
            }
            else
            {
                wildCardCount -= gap;
            }
        }
    }

    return true;
}


static bool HasFourOfAKind(List<Card> hand, int wildCardCount)
{
    // ランクごとにカードをグループ化
    var rankGroups = hand.GroupBy(card => card.Rank);

    // ワイルドカードを使用してフォーカードの役が存在するかどうかを判定
    foreach (var group in rankGroups)
    {
        if (group.Count() + wildCardCount >= 4)
        {
            return true;
        }
    }

    return false;
}

static bool HasFullHouse(List<Card> hand, int wildCardCount)
{
    // ランクごとにカードをグループ化
    var rankGroups = hand.GroupBy(card => card.Rank);

    int threeOfAKindCount = 0;
    int pairCount = 0;

    foreach (var group in rankGroups)
    {
        int groupCount = group.Count();
        if (groupCount >= 3)
        {
            threeOfAKindCount++;
        }
        else if (groupCount >= 2)
        {
            pairCount++;
        }
    }

    // ワイルドカードの利用回数を考慮してフルハウスの判定を行う
    if (threeOfAKindCount >= 1 && pairCount >= 1)
    {
        return true;
    }
    else if (threeOfAKindCount == 0 && pairCount >= 2 && wildCardCount >= 1)
    {
        return true;
    }
    else if (threeOfAKindCount >= 1 && pairCount == 0 && wildCardCount >= 2)
    {
        return true;
    }

    return false;
}



static bool HasThreeOfAKind(List<Card> hand, int wildCardCount)
{
    // ランクごとにカードをグループ化
    var rankGroups = hand.GroupBy(card => card.Rank);

    // ワイルドカードを使用してスリーカードの役が存在するかどうかを判定
    foreach (var group in rankGroups)
    {
        if (group.Count() + wildCardCount >= 3)
        {
            return true;
        }
    }

    return false;
}



    }

    enum HandRank
    {
        HighCard,
        OnePair,
        TwoPair,
        ThreeOfAKind,
        Straight,
        Flush,
        FullHouse,
        FourOfAKind,
        StraightFlush,
        RoyalFlush
    }
}
