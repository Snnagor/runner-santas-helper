using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

#region Road
[System.Serializable]
public class RoadClass
{
    [SerializeField] private int countRoad;
    public int CountRoad { get => countRoad;}

    [SerializeField] private Road prefab;
    public Road Prefab { get => prefab; }
}


[System.Serializable]
public class RoadScene
{
    [SerializeField] private List<Road> _roads = new List<Road>();   
    public List<Road> Roads { get => _roads; set => _roads = value; }
}

#endregion

#region Coin
[System.Serializable]
public class CoinsClass
{
    [SerializeField] private int countCoin;
    public int CountCoin { get => countCoin; }

    [SerializeField] private Coin prefab;
    public Coin Prefab { get => prefab; }
}
#endregion

#region Block

[System.Serializable]
public class BlockClass
{
    [SerializeField] private int countBlock;
    public int CountBlock { get => countBlock; }

    [SerializeField] private Block prefab;
    public Block Prefab { get => prefab; }
}

[System.Serializable]
public class BlockScene
{
    [SerializeField] private List<Block> _blocks = new List<Block>();
    public List<Block> Blocks { get => _blocks; set => _blocks = value; }
}

#endregion

#region Empty

[System.Serializable]
public class EmptyClass
{
    [SerializeField] private int countEmpty;
    public int CountEmpty { get => countEmpty; }

    [SerializeField] private Empty prefab;
    public Empty Prefab { get => prefab; }
}

#endregion

#region EmptyFlag

[System.Serializable]
public class EmptyFlagClass
{
    [SerializeField] private int countEmptyFlag;
    public int CountEmptyFlag { get => countEmptyFlag; }

    [SerializeField] private Empty prefab;
    public Empty Prefab { get => prefab; }
}

#endregion

#region Gifts

[System.Serializable]
public class GiftClass
{
    [SerializeField] private int countGift;
    public int CountGift { get => countGift; }

    [SerializeField] private Gift prefab;
    public Gift Prefab { get => prefab; }
}


[System.Serializable]
public class GiftScene
{
    [SerializeField] private Gift _gifts;
    public Gift Gifts { get => _gifts; set => _gifts = value; }
}

#endregion

public class Spawner : MonoBehaviour
{
    [SerializeField] private List<RoadClass> roads;
    [SerializeField] private List<RoadScene> roadOnScene;
    public List<RoadScene> RoadOnScene
    {
        get
        {
            return roadOnScene;
        }
    }

    [SerializeField] private List<GiftClass> gifts;
    [SerializeField] private List<Gift> giftOnScene;
    public List<Gift> GiftOnScene
    {
        get
        {
            return giftOnScene;
        }
    }

    [SerializeField] private CoinsClass coins;
    [SerializeField] private List<Coin> coinOnScene;
    public List<Coin> CoinOnScene
    {
        get
        {
            return coinOnScene;
        }
    }

    [SerializeField] private List<BlockClass> blocks;
    [SerializeField] private List<BlockScene> blockOnScene;
    public List<BlockScene> BlockOnScene
    {
        get
        {
            return blockOnScene;
        }
    }

    [SerializeField] private EmptyClass empties;
    [SerializeField] private List<Empty> emptyOnScene;
    public List<Empty> EmptyOnScene
    {
        get
        {
            return emptyOnScene;
        }
    }

    [SerializeField] private EmptyClass emptiesFlag;
    [SerializeField] private List<Empty> emptyFlagOnScene;
    public List<Empty> EmptyFlagOnScene
    {
        get
        {
            return emptyFlagOnScene;
        }
    }

    [Space]
    [Header("Parents For Spawn")]
    [SerializeField] private Transform roadsParent;
    [SerializeField] private Transform coinsParent;
    [SerializeField] private Transform blocksParent;
    [SerializeField] private Transform emptyParent;
    [SerializeField] private Transform giftParent;

    #region Injects
    private DiContainer diContainer;

    [Inject]
    private void Construct(Config _config, DiContainer _diContainer)
    {        
        diContainer = _diContainer;
    }

    #endregion

    private void Awake()
    {       
        CreateRoad();
        CreateCoin();
        CreateBlock();
        CreateEmpty();
        CreateGift();
        CreateEmptyFlag();
    }

    private void CreateRoad()
    {
        for (int i = 0; i < roads.Count; i++)
        {
            roadOnScene.Add(new RoadScene());

            for (int j = 0; j < roads[i].CountRoad; j++)
            {
                Road roadObject = diContainer.InstantiatePrefabForComponent<Road>(roads[i].Prefab, Vector3.zero, Quaternion.identity, roadsParent);
                roadObject.gameObject.SetActive(false);
                roadOnScene[i].Roads.Add(roadObject);
            }
        }
    }

    private void CreateGift()
    {
        for (int i = 0; i < gifts.Count; i++)
        {

            for (int j = 0; j < gifts[i].CountGift; j++)
            {
                Gift giftObject = diContainer.InstantiatePrefabForComponent<Gift>(gifts[i].Prefab, Vector3.zero, Quaternion.identity, giftParent);
                giftObject.gameObject.SetActive(false);
                giftOnScene.Add(giftObject);
            }
        }

        ShuffleGiftList(giftOnScene);
    }

    private void CreateCoin()
    {
        for (int j = 0; j < coins.CountCoin; j++)
        {
            Coin coinObject = diContainer.InstantiatePrefabForComponent<Coin>(coins.Prefab, Vector3.zero, Quaternion.identity, coinsParent);
            coinObject.gameObject.SetActive(false);
            coinOnScene.Add(coinObject);
        }
    }

    private void CreateBlock()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            blockOnScene.Add(new BlockScene());

            for (int j = 0; j < blocks[i].CountBlock; j++)
            {
                Block blockObject = diContainer.InstantiatePrefabForComponent<Block>(blocks[i].Prefab, Vector3.zero, Quaternion.identity, blocksParent);                
                blockOnScene[i].Blocks.Add(blockObject);
                blockObject.gameObject.SetActive(false);
            }
        }        
    }

    private void CreateEmpty()
    {
        for (int j = 0; j < empties.CountEmpty; j++)
        {
            Empty emptyObject = diContainer.InstantiatePrefabForComponent<Empty>(empties.Prefab, Vector3.zero, Quaternion.identity, emptyParent);
            emptyObject.gameObject.SetActive(false);
            emptyOnScene.Add(emptyObject);
        }
    }

    private void CreateEmptyFlag()
    {
        for (int j = 0; j < emptiesFlag.CountEmpty; j++)
        {
            Empty emptyObject = diContainer.InstantiatePrefabForComponent<Empty>(emptiesFlag.Prefab, Vector3.zero, Quaternion.identity, emptyParent);
            emptyObject.gameObject.SetActive(false);
            emptyFlagOnScene.Add(emptyObject);
        }
    }

    /// <summary>
    /// Перемешивание List-а с подарками
    /// </summary>
    /// <param name="list"></param>
    public void ShuffleGiftList(List<Gift> list)
    {
        for (int i = list.Count - 1; i >= 1; i--)
        {
            int j = Random.Range(0, i + 1);

            Gift tmp = list[j];
            list[j] = list[i];
            list[i] = tmp;
        }
    }

}
