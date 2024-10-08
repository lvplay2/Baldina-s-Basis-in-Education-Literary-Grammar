using UnityEngine;

public class GameplayManager : MonoBehaviour
{
	public enum DeviceType
	{
		Mobile = 0,
		PC = 1
	}

	public enum PlatformType
	{
		Windows = 0,
		Android = 1,
		iOS = 2
	}

	public DeviceType currentDevice;

	public PlatformType currentPlayform;

	public bool isChristmasIntro;

	public bool isSlowBaldina;

	public bool isEndlessStamina;

	public bool isImmortalityMode;

	public bool isUnlimiterAll;

	private float _sensitivity;

	private bool _isAudio;

	[Header("Скорость персонажа ходьбы")]
	public float playerMoveSpeed;

	[Header("Скорость персонажа бег")]
	public float playerRunSpeed;

	[Space(30f)]
	[Header("Время перемены перед началом игры")]
	public float baldina_BreakTime;

	[Header("Скорость Балдины")]
	public float baldina_NavMeshSpeed;

	[Header("Длина шага")]
	public float baldina_StepTime;

	[Header("Задержка между шагами")]
	public Vector2 baldina_MinMaxStepDelay;

	[Header("Время действия книги на балдину")]
	public float baldina_BookTime;

	[Header("Скорость Балдины с бустом SLOW BALDINA")]
	public float baldina_NavMeshSpeedSlow;

	[Space(30f)]
	[Header("Скорость девочки")]
	public int girl_navSpeed;

	[Header("Время деактивации девочки")]
	public int girl_relaxTime;

	[Header("Количество примеров девочки")]
	public int girl_exampleCount;

	[Space(30f)]
	[Header("Скорость директора")]
	public int director_navSpeed;

	[Header("Время наказания директора")]
	public int director_punishTime;

	[Header("Время действия газеты на директора")]
	public float director_NewspaperTime;

	[Space(30f)]
	[Header("Скорость уборщицы")]
	public int cleaner_navSpeed;

	[Header("Мин/макс время жизни воды")]
	public Vector2 cleaner_minMaxWater;

	[Header("Время наказания в воде")]
	public int cleaner_punishTime;

	[Header("Время действия ботинок")]
	public int bootsTime;

	[Space(30f)]
	[Header("Скорость булли")]
	public int bully_navSpeed;

	[Header("Время смены комнаты Булли")]
	public int bully_changeClassTime;

	[Header("Время наказания Булли")]
	public int bully_punishTime;

	[Header("Время деактивации Булли")]
	public int bully_relaxTime;

	[Space(30f)]
	[Header("Время наказания Охранника")]
	public int security_punishTime;

	[Header("Время сколько стоит Охранник")]
	public int security_stayTime;

	[Space(30f)]
	[Header("Скорость черлидерши")]
	public int queen_navSpeed;

	[Header("Время окаменения черлидерши")]
	public int queen_stoneTime;

	[Space(30f)]
	[Header("Скорость футболиста")]
	public int thed_navSpeed;

	[Header("Скорость бега футболиста")]
	public int thed_navSpeedRun;

	[Header("Время сбития игрока без мяча футболистом")]
	public int thed_punishTime;

	[Header("Время сбития игрока с мячом футболистом")]
	public int thed_punishTimeBall;

	[Header("Время сбития нпс футболистом")]
	public int thed_npcPunishTime;

	[Space(30f)]
	[Header("Время на решение пазла")]
	public int puzzleTime;

	public static GameplayManager This { get; private set; }

	public float sensitivity
	{
		get
		{
			return _sensitivity;
		}
		set
		{
			_sensitivity = value;
			PlayerPrefs.SetFloat("Sensitivity", value);
		}
	}

	public bool isAudio
	{
		get
		{
			return _isAudio;
		}
		set
		{
			_isAudio = value;
			PlayerPrefs.SetFloat("IsAudio", value ? 1 : 0);
		}
	}

	private void Awake()
	{
		This = this;
		sensitivity = PlayerPrefs.GetFloat("Sensitivity", 3f);
		isAudio = ((PlayerPrefs.GetInt("IsAudio", 1) != 0) ? true : false);
		Debug.Log(sensitivity);
	}

	public void MY_SwitchCursor(bool isOn)
	{
		if (currentDevice == DeviceType.PC)
		{
			Cursor.visible = isOn;
			Cursor.lockState = ((!isOn) ? CursorLockMode.Locked : CursorLockMode.None);
		}
	}
}
