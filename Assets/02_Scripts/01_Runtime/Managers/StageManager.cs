using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MinD.Runtime;
using MinD.Runtime.Entity;
using MinD.Runtime.Managers;
using MinD.Runtime.UI;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class StageManager : Singleton<StageManager>
{
	private const float CHANGESTAGE_FADEDURATION_IN = 1.5f;
	private const float CHANGESTAGE_FADEDURATION_OUT = 1f;
	
	[Serializable]
	private struct StageSetting
	{
		public string stageName;
		public Transform stageStartPlayerPosition;
		public GameObject stageParent;
		[FormerlySerializedAs("onStageEntered")] [FormerlySerializedAs("onChanged")] public UnityEvent onEntered;
	}

	[SerializeField] private StageSetting[] m_stageSettings;
	[SerializeField] private string m_firstStage;
	
	private WaitForSeconds m_changeStageFadeInYield = new WaitForSeconds(CHANGESTAGE_FADEDURATION_IN);
	private WaitForSeconds m_changeStageFadeDelayYield = new WaitForSeconds(1f);
	private WaitForSeconds m_changeStageFadeOutYield = new WaitForSeconds(CHANGESTAGE_FADEDURATION_OUT);

	private void Start()
	{
		ChangeStage(m_firstStage);
	}

	public void ChangeStage(string stageName, float delay = 0)
	{
		StageSetting safeFirstStage = m_stageSettings.First(stage => stage.stageName.Equals(stageName));
		if (safeFirstStage.stageStartPlayerPosition != null)
		{
			StartCoroutine(ChangeStageCoroutine(safeFirstStage, delay));
		}
	}
	
	private IEnumerator ChangeStageCoroutine(StageSetting stage, float delay)
	{
		yield return new WaitForSeconds(delay);

		if (!stage.stageName.Equals(m_firstStage))
		{
			PlayerHUDManager.Instance.FadeInToBlack(CHANGESTAGE_FADEDURATION_IN);
			Player.player.canMove = false;
			Player.player.canRotate = false;
			yield return m_changeStageFadeInYield;
		}
		
		stage.onEntered?.Invoke();

		foreach (StageSetting s in m_stageSettings)
		{	
			s.stageParent.gameObject.SetActive(s.stageName.Equals(stage.stageName));
		}
		
		Player.player.cc.enabled = false;
		stage.stageStartPlayerPosition.GetPositionAndRotation(out var position, out var rotation);
		Player.player.transform.SetPositionAndRotation(position, rotation);
		Player.player.cc.enabled = true;

		if (!stage.stageName.Equals(m_firstStage))
		{
			yield return m_changeStageFadeDelayYield;
		}
		
		PlayerHUDManager.Instance.FadeOutFromBlack(CHANGESTAGE_FADEDURATION_OUT);
		yield return m_changeStageFadeOutYield;
		Player.player.canMove = true;
		Player.player.canRotate = true;
	}
}
