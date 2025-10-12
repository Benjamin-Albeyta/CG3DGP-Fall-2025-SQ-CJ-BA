/**
  * Author: Benjamin Albeyta
  * Date Created: 10/11/2025
  * Date Last Updated: 10/11/2025
  * Summary: When attached to an object creates a death plane (touch = run player death)
  */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathPlane : MonoBehaviour
{
  //On collision with the deathplane, activate the GameManager for player died.
  private void OnCollisionEnter(Collision collision)
  {
    GameManager.Instance.PlayerDied();
  }
}
