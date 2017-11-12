/// <summary>
/// 
/// /// 11/1/2013
/// Steve Peters
/// GameDevelopersGuild.com
/// Miami Fl
/// 
/// Monitors the framerate of the application and modifies break and remove booleans based on this.
/// All breakable objects check against _dlXBreak to see if they should be allowed to break.
/// If framerate drops below a calculated amount per level, then the objects of that level are removed from the scene.
/// This makes the objects dissapear under heavy load, but prevents most excessive framerate drops. 
/// 
/// An added optimization is the lowering of the physics solver iteration count as framerate decreases. This 
/// lowers the quality of the physics calculations when framerate is low, but speeds things up a bit.
/// 
/// FPS display code modified from
///  http://wiki.unity3d.com/index.php?title=FramesPerSecond
///  Aras Pranckevicius (NeARAZ) 
///  cc 3.0
/// </summary>
using UnityEngine;
using System.Collections;

public class FPSController_Singleton : MonoBehaviour
{
	
	//framerate variables
	public bool displayFPS = false;
	public int minFPS = 15;
	protected float frameRate;
	public  float updateInterval = 0.5F;
	private float accum = 0; // FPS accumulated over the interval
	private int   frames = 0; // Frames drawn over the interval
	private float timeleft; // Left time for current interval
	
	public removabilityState removalState;
	public bool canBreakDL0;
	public bool canBreakDL1;
	public bool canBreakDL2;
	
	private static FPSController_Singleton instance;
	private int _dl0Break;
	private int _dl1Break;
	private int _dl2Break;
	private int _dl1Remove;
	private int _dl2Remove;
	private int _dl3Remove;

	private float _BreakSoundDelayTime;
	const float BREAKSOUNDDELAY = 0.25f;
	
	public static FPSController_Singleton Instance {
		get {
			if (instance == null) {
				//Debug.LogWarning ("No singleton exists! Creating new one.");
				GameObject owner = new GameObject ("FPSController_Singleton");
				instance = owner.AddComponent<FPSController_Singleton> ();
			}
			return instance;
		}
	}
 
	private void Awake ()
	{
		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (instance != this) {
			//Debug.LogWarning ("A singleton already exists! Destroying new one.");
			Destroy (this);
		}
		
		//set initial removal variables
		_dl0Break = minFPS;
		_dl1Break = minFPS * 2;
		_dl2Break = minFPS * 3;
		
		_dl1Remove = minFPS;
		_dl2Remove = minFPS * 2;
		_dl3Remove = minFPS * 3;
 
	}
	
	/**
	 * The point at which destructible sub-objects are removed to improve performance.
	 * This is intended to be the lowest target framerate. owest level objects will be 
	 * removed 50% higher, Land second level objects will be removed 25% higher
	 */ 
	protected void Update ()
	{
		// Calculate the framrate ands reduce the number of physics calculations to help slower processors cope.
		//frameRate = 1 / Time.smoothDeltaTime;
		CalculateFramerate ();
		
		// Performance manager. Lowers the number of physics calculations as framrate decreases. Max is 7
		if (frameRate < 70)
		{
			Physics.defaultSolverIterations = (int) (frameRate / 10);
		}
		
		else 
		{
			Physics.defaultSolverIterations = 7;
		}
		
		SetBreakabilityState ();
		SetRemovalState ();
	}
	
	/* modified from Unity Wiki
	 * http://wiki.unity3d.com/index.php?title=FramesPerSecond
	 * Aras Pranckevicius (NeARAZ) 
     * cc 3.0
	 */
	
	private void CalculateFramerate ()
	{
		timeleft -= Time.deltaTime;
		accum += Time.timeScale / Time.deltaTime;
		++frames;
 
		// Interval ended - update GUI text and start new interval
		if (timeleft <= 0.0) 
		{
			// display two fractional digits (f2 format)
			frameRate = accum / frames;
 
	
			timeleft = updateInterval;
			accum = 0.0F;
			frames = 0;
		}
	}
 
	public int GetFPS ()
	{
		return (int)frameRate;
	}
	
	private void SetBreakabilityState ()
	{
		if (frameRate < _dl0Break) 
		{
			canBreakDL0 = false;
			canBreakDL1 = false;
			canBreakDL2 = false;
		} 
		else if (frameRate < _dl1Break) 
		{
			canBreakDL0 = true;
			canBreakDL1 = false;
			canBreakDL2 = false;
		} 
		else if (frameRate < _dl2Break) 
		{
			canBreakDL0 = true;
			canBreakDL1 = true;
			canBreakDL2 = false;
		} 
		else 
		{
			canBreakDL0 = true;
			canBreakDL1 = true;
			canBreakDL2 = true;
		}
	}
	
	private void SetRemovalState ()
	{
		if (frameRate < _dl1Remove) {
			removalState = removabilityState.removeDL1;
		} else if (frameRate < _dl2Remove) {
			removalState = removabilityState.removeDL2;
		} else if (frameRate < _dl3Remove) {
			removalState = removabilityState.removeDL3;
		} else {
			removalState = removabilityState.noRemoval;
		}
	}
	
	void OnGUI ()
	{
		if (displayFPS) 
		{
			GUI.Box (new Rect (Screen.width / 2, 0, 100, 25), GetFPS().ToString ());

		}
	}

	/// <summary>
	/// Determines whether this instance can play breaking sound, if enough time has passed since the last sound was played
	/// </summary>
	/// <returns><c>true</c> if this instance can play breaking sound; otherwise, <c>false</c>.</returns>
	public bool CanPlayBreakingSound()
	{
		if(Time.time > _BreakSoundDelayTime)
		{
			_BreakSoundDelayTime = Time.time + BREAKSOUNDDELAY;
			return true;
		}

		else
		{
			return false;
		}
	}
	
	//determines if objects should be removed due to low framerate
	public enum removabilityState
	{
		noRemoval,
		removeDL1,
		removeDL2,
		removeDL3
	}
}
