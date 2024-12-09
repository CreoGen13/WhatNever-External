// using System;
// using System.Collections.Generic;
// using Game;
// using Mechanics;
// using Node.Panel;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Infrastructure.Services
// {
//     public class SpawnManager: MonoBehaviour
//     {
//         [SerializeField] private GameObject bubblePrefab;
//         [SerializeField] private Transform [] stages;
//         private Queue<GameSprite.GameSpriteView> _sprites;
//
//         public void InitializePool(int count)
//         {
//             if (_sprites == null)
//             {
//                 _sprites = new Queue<GameSprite.GameSpriteView>();
//             }
//             foreach (var sprite in _sprites)
//             {
//                 Destroy(sprite.gameObject);
//             }
//             _sprites.Clear();
//             
//             //_sprites = new Queue<GameSprite>();
//             for(int i = 0; i < count; i++)
//             {
//                 var currentBubble = Instantiate(bubblePrefab, transform).GetComponent<GameSprite.GameSpriteView>();
//                 currentBubble.gameObject.SetActive(false);
//                 _sprites.Enqueue(currentBubble);
//             }
//         }
//         public GameSprite.GameSpriteView SpawnBubble(NodeData moveNodeData, Sprite sprite)
//         {
//             GameSprite.GameSpriteView gameSprite = _sprites.Dequeue();
//             gameSprite.SetDefaultSize(false);
//             GameObject go = gameSprite.gameObject;
//             go.SetActive(true);
//             go.transform.SetParent(stages[5]);
//             gameSprite.SetBubble(sprite, moveNodeData.localizedText.GetLocalizedString(), moveNodeData.arrivalType, moveNodeData.color);
//             gameSprite.SetTransform(moveNodeData.spritePosition, moveNodeData.scaleAndRotation, true);
//
//             return gameSprite;
//         }
//         public GameSprite.GameSpriteView SpawnAndActivateBubble(NodeData moveNodeData, Sprite sprite, Action callback, bool waitForAnimation)
//         {
//             var gameSprite = SpawnBubble(moveNodeData, sprite);
//             
//             if(waitForAnimation)
//                 gameSprite.SetCallback(callback);
//             
//             gameSprite.Activate();
//
//             return gameSprite;
//         }
//         public GameSprite.GameSpriteView SpawnSprite(NodeData moveNodeData)
//         {
//             GameSprite.GameSpriteView gameSprite = _sprites.Dequeue();
//             GameObject go = gameSprite.gameObject;
//             go.SetActive(true);
//             SetStage(go, moveNodeData.stage);
//             gameSprite.SetSprite(moveNodeData.sprite, moveNodeData.spriteTag, moveNodeData.arrivalType, moveNodeData.color);
//             gameSprite.SetTransform(moveNodeData.spritePosition, moveNodeData.scaleAndRotation, false, moveNodeData.stage);
//             gameSprite.SetDefaultSize(true);
//
//             return gameSprite;
//         }
//         public GameSprite.GameSpriteView SpawnAndActivateSprite(NodeData moveNodeData, Action callback, bool waitForAnimation)
//         {
//             GameSprite.GameSpriteView gameSprite = _sprites.Dequeue();
//             GameObject go = gameSprite.gameObject;
//             go.SetActive(true);
//             SetStage(go, moveNodeData.stage);
//             gameSprite.SetSprite(moveNodeData.sprite, moveNodeData.spriteTag, moveNodeData.arrivalType, moveNodeData.color);
//             gameSprite.SetTransform(moveNodeData.spritePosition, moveNodeData.scaleAndRotation, false, moveNodeData.stage);
//             gameSprite.SetDefaultSize(true);
//             
//             //if(waitForAnimation)
//             gameSprite.SetCallback(callback);
//             
//             gameSprite.Activate();
//
//             return gameSprite;
//         }
//         public GameSprite.GameSpriteView SpawnAndMoveSprite(NodeData moveNodeData, Action callback, GameSprite.GameSpriteView lastSprite, bool waitForAnimation)
//         {
//             GameSprite.GameSpriteView gameSprite = _sprites.Dequeue();
//             GameObject go = gameSprite.gameObject;
//             go.SetActive(true);
//             SetStage(go, moveNodeData.stage);
//             gameSprite.ChangeAndMove(moveNodeData, callback, this, lastSprite, waitForAnimation);
//
//             return gameSprite;
//         }
//         public GameObject SpawnGesture(NodeData moveNodeData, GestureMechanics gestureMechanics, Action callback)
//         {
//             var pictureGameObject = Instantiate(
//                 moveNodeData.gameObject,
//                 moveNodeData.position,
//                 Quaternion.Euler(0, 0, moveNodeData.scaleAndRotation.z));
//             SetStage(pictureGameObject, moveNodeData.stage);
//             RecognitionPicture picture = pictureGameObject.GetComponent<RecognitionPicture>();
//             picture.Activate(moveNodeData, callback);
//             
//             gestureMechanics.SetPicture(picture);
//
//             return pictureGameObject;
//         }
//
//         public void ReturnSprite(GameSprite.GameSpriteView gameSprite)
//         {
//             if (gameSprite.HasTrigger)
//                 Destroy(gameSprite.GetComponent<Button>());
//             if (gameSprite.HasAnimation)
//                 Destroy(gameSprite.GetComponent<Animator>());
//             
//             
//             gameSprite.ResetFlags();
//             gameSprite.transform.SetParent(transform);
//             gameSprite.gameObject.SetActive(false);
//
//             _sprites.Enqueue(gameSprite);
//         }
//         public void SetStage(GameObject go, Stage stage)
//         {
//             switch (stage)
//             {
//                 case Stage.First:
//                 {
//                     go.transform.SetParent(stages[0]);
//                     break;
//                 }
//                 case Stage.Second:
//                 {
//                     go.transform.SetParent(stages[1]);
//                     break;
//                 }
//                 case Stage.Third:
//                 {
//                     go.transform.SetParent(stages[2]);
//                     break;
//                 }
//                 case Stage.Fourth:
//                 {
//                     go.transform.SetParent(stages[3]);
//                     break;
//                 }
//                 case Stage.Fifth:
//                 {
//                     go.transform.SetParent(stages[4]);
//                     break;
//                 }
//             }
//         }
//     }
// }