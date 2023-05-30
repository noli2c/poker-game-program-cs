using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerGame
{
    /// <summary>
    /// スートを表す列挙型
    /// </summary>
    enum Suit
    {
        Hearts,    // ハート
        Diamonds,  // ダイヤ
        Clubs,     // クラブ
        Spades,    // スペード
        Joker      // ジョーカー
    }

    /// <summary>
    /// ランクを表す列挙型
    /// </summary>
    enum Rank
    {
        Ace = 1,   // エース
        Two,       // 2
        Three,     // 3
        Four,      // 4
        Five,      // 5
        Six,       // 6
        Seven,     // 7
        Eight,     // 8
        Nine,      // 9
        Ten,       // 10
        Jack,      // ジャック
        Queen,     // クイーン
        King,      // キング
        Joker      // ジョーカー
    }

    /// <summary>
    /// 役を表す列挙型
    /// </summary>
    enum HandRank
    {
        HighCard,        // ハイカード
        OnePair,         // ワンペア
        TwoPair,         // ツーペア
        ThreeOfAKind,    // スリーカード
        Straight,        // ストレート
        Flush,           // フラッシュ
        FullHouse,       // フルハウス
        FourOfAKind,     // フォーカード
        StraightFlush,   // ストレートフラッシュ
        RoyalFlush       // ロイヤルフラッシュ
    }

    /// <summary>
    /// トランプのカードを表す構造体
    /// </summary>
    struct Card
    {
        public Suit Suit;   // スート
        public Rank Rank;   // ランク
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

        /// <summary>
        /// ポーカーゲームをプレイするメソッド
        /// </summary>
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

        /// <summary>
        /// トランプのデッキを作成するメソッド
        /// </summary>
        /// <returns>デッキのリスト</returns>
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

        /// <summary>
        /// デッキをシャッフルするメソッド
        /// </summary>
        /// <param name="deck">シャッフルするデッキ</param>
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

        /// <summary>
        /// 手札を配布するメソッド
        /// </summary>
        /// <param name="deck">デッキ</param>
        /// <param name="numCards">配布するカードの枚数</param>
        /// <returns>配布された手札のリスト</returns>
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

        /// <summary>
        /// デッキからカードを引くメソッド
        /// </summary>
        /// <param name="deck">デッキ</param>
        /// <returns>引かれたカード</returns>
        static Card DrawCard(List<Card> deck)
        {
            Card card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// 手札を表示するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        static void DisplayHand(List<Card> hand)
        {
            for (int i = 0; i < hand.Count; i++)
            {
                Card card = hand[i];
                Console.WriteLine((i + 1) + ": " + GetCardString(card));
            }
        }

        /// <summary>
        /// カードの文字列表現を取得するメソッド
        /// </summary>
        /// <param name="card">カード</param>
        /// <returns>カードの文字列表現</returns>
        static string GetCardString(Card card)
        {
            if (card.Rank == Rank.Joker)
            {
                return "Joker";
            }

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

        /// <summary>
        /// 手札の役を判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <returns>役のランク</returns>
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

        /// <summary>
        /// 手札がツーペアの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ツーペアの役を持っているかどうか</returns>
        static bool HasTwoPair(List<Card> hand, int wildCardCount)
        {
            // ランクごとにカードをグループ化
            var rankGroups = hand.GroupBy(card => card.Rank);

            // ペアの数を数える
            int pairCount = rankGroups.Count(group => group.Count() >= 2);

            // ワイルドカードを使用してツーペアの役が存在するかどうかを判定
            return pairCount >= 2 || (pairCount == 1 && wildCardCount >= 1);
        }

        /// <summary>
        /// 手札がワンペアの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ワンペアの役を持っているかどうか</returns>
        static bool HasOnePair(List<Card> hand, int wildCardCount)
        {
            // ランクごとにカードをグループ化
            var rankGroups = hand.GroupBy(card => card.Rank);

            // ワイルドカードを使用してペアの役が存在するかどうかを判定
            return rankGroups.Any(group => group.Count() + wildCardCount >= 2);
        }

        /// <summary>
        /// 手札がロイヤルフラッシュの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ロイヤルフラッシュの役を持っているかどうか</returns>
        static bool HasRoyalFlush(List<Card> hand, int wildCardCount)
        {
            // カードをスートごとにグループ化
            var suitGroups = hand.GroupBy(card => card.Suit);

            // スートごとにランクの配列を取得
            var rankArrays = suitGroups.Select(group => group.Select(card => card.Rank).ToArray()).ToList();

            // ロイヤルフラッシュの役を構成するランク
            Rank[] royalFlushRanks = { Rank.Ten, Rank.Jack, Rank.Queen, Rank.King, Rank.Ace };

            // ワイルドカードを使用してロイヤルフラッシュの役が存在するかどうかを判定
            return rankArrays.Any(ranks => ranks.Length + wildCardCount >= 5 && royalFlushRanks.All(rank => ranks.Contains(rank) || wildCardCount > 0));
        }

        /// <summary>
        /// 手札がストレートフラッシュの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ストレートフラッシュの役を持っているかどうか</returns>
        static bool HasStraightFlush(List<Card> hand, int wildCardCount)
        {
            // カードをスートごとにグループ化
            var suitGroups = hand.GroupBy(card => card.Suit);

            // スートごとにランクの配列を取得
            var rankArrays = suitGroups.Select(group => group.Select(card => card.Rank).ToArray()).ToList();

            // ワイルドカードを使用してストレートフラッシュの役が存在するかどうかを判定
            return rankArrays.Any(ranks => ranks.Length + wildCardCount >= 5 && IsStraight(ranks, wildCardCount));
        }

        /// <summary>
        /// ランクの配列がストレートの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="ranks">ランクの配列</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ストレートの役を持っているかどうか</returns>
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

        /// <summary>
        /// 手札がフラッシュの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>フラッシュの役を持っているかどうか</returns>
        static bool HasFlush(List<Card> hand, int wildCardCount)
        {
            // カードをスートごとにグループ化
            var suitGroups = hand.GroupBy(card => card.Suit);

            // ワイルドカードを使用してフラッシュの役が存在するかどうかを判定
            return suitGroups.Any(group => group.Count() >= 5 || (group.Count() + wildCardCount >= 5));
        }

        /// <summary>
        /// 手札がストレートの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>ストレートの役を持っているかどうか</returns>
        static bool HasStraight(List<Card> hand, int wildCardCount)
        {
            var ranks = hand.Select(card => card.Rank).Distinct().ToList();
            ranks.Sort();

            // ストレートの役が存在するかどうかを判定
            int consecutiveCount = 0;

            for (int i = 0; i < ranks.Count - 1; i++)
            {
                if (ranks[i] + 1 == ranks[i + 1])
                {
                    consecutiveCount++;
                }
                else if (ranks[i] != ranks[i + 1])
                {
                    int gap = ranks[i + 1] - ranks[i] - 1;
                    if (gap > wildCardCount)
                    {
                        consecutiveCount = 0;
                    }
                    else
                    {
                        consecutiveCount += gap;
                        wildCardCount -= gap;
                    }
                }

                if (consecutiveCount + wildCardCount >= 4)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// 手札がフォーカードの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>フォーカードの役を持っているかどうか</returns>
        static bool HasFourOfAKind(List<Card> hand, int wildCardCount)
        {
            // ランクごとにカードをグループ化
            var rankGroups = hand.GroupBy(card => card.Rank);

            // ワイルドカードを使用してフォーカードの役が存在するかどうかを判定
            return rankGroups.Any(group => group.Count() + wildCardCount >= 4);
        }

        /// <summary>
        /// 手札がフルハウスの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>フルハウスの役を持っているかどうか</returns>
        static bool HasFullHouse(List<Card> hand, int wildCardCount)
        {
            // ランクごとにカードをグループ化
            var rankGroups = hand.GroupBy(card => card.Rank);

            int threeOfAKindCount = rankGroups.Count(group => group.Count() >= 3);
            int pairCount = rankGroups.Count(group => group.Count() >= 2);

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

        /// <summary>
        /// 手札がスリーカードの役を持っているかどうか判定するメソッド
        /// </summary>
        /// <param name="hand">手札</param>
        /// <param name="wildCardCount">ワイルドカードの枚数</param>
        /// <returns>スリーカードの役を持っているかどうか</returns>
        static bool HasThreeOfAKind(List<Card> hand, int wildCardCount)
        {
            // ランクごとにカードをグループ化
            var rankGroups = hand.GroupBy(card => card.Rank);

            // ワイルドカードを使用してスリーカードの役が存在するかどうかを判定
            return rankGroups.Any(group => group.Count() + wildCardCount >= 3);
        }
    }
}
