using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Models
{
  public class MainModel
  {

    public event Action OnUpdate;
    
    private Dictionary<ulong, User> _idToUser = new Dictionary<ulong, User>();
    private bool _dispatchActive = true;
    public User[] Users => _idToUser.Values.ToArray();

    public bool TryGetUser(ulong id, out User user) => _idToUser.TryGetValue(id, out user);
    
    public IEnumerable<User> GetUsersWithState(User.State state)
    {
      foreach (var user in _idToUser.Values)
      {
        if (user.CurState == state)
        {
          yield return user;
        }
      }
    }

    public void StopDispatch()
    {
      _dispatchActive = false;
    }

    public void StartDispatch()
    {
      _dispatchActive = true;
      DispatchUpdate();
    }
    
    public void RemoveConnectedClient(ulong clientId)
    {
      if (!_idToUser.ContainsKey(clientId))
      {
        return;
      }

      _idToUser.Remove(clientId);
      DispatchUpdate();
    }

    public void DispatchUpdate()
    {
      if (!_dispatchActive)
      {
        return;
      }
      OnUpdate?.Invoke();
    }
    
    public void UpdateUserState(ulong id, User.State state)
    {
      if (!_idToUser.TryGetValue(id, out var player))
      {
        player = new User(id);
        _idToUser[id] = player;
      }

      player.CurState = state;
      DispatchUpdate();
      Debug.Log($"player: {id} state: {state}");
    }

    public void UpdateUsersStates(User[] players)
    {
      StopDispatch();
      foreach (var player in players)
      {
        UpdateUserState(player.Id, player.CurState);
      }
      StartDispatch();
    }

    public void Clear()
    {
      _idToUser.Clear();
    }
  }
}