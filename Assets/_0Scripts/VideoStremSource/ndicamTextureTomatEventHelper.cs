

//ndicamTextureTomatEventHelper 
#if !OPENCV_DONT_USE_WEBCAMTEXTURE_API
#if !(PLATFORM_LUMIN && !UNITY_EDITOR)

using OpenCVForUnity.CoreModule;
using OpenCVForUnity.ImgprocModule;
using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.UtilsModule;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;


/// <summary>
/// NDIcamTexture to mat helper.
/// v 1.1.4
/// </summary>
/// 


namespace OpenCVForUnityExample
{
    public class ndicamTextureTomatEventHelper : MonoBehaviour
    {



        /// <summary>
        /// Set the name of the camera device to use. (or device index number)
        /// </summary>
        [SerializeField, FormerlySerializedAs("requestedDeviceName"), TooltipAttribute("Set the name of the device to use. (or device index number)")]
        protected string _requestedDeviceName = null;

        public string requestedDeviceName
        {
            get { return _requestedDeviceName; }
            set
            {
                if (_requestedDeviceName != value)
                {
                    _requestedDeviceName = value;
                    if (hasInitDone)
                        Initialize();
                }
            }
        }

        /// <summary>
        /// Set the width of camera.
        /// </summary>
        // [SerializeField, FormerlySerializedAs("requestedWidth"), TooltipAttribute("Set the width of camera.")]
        protected int _requestedWidth = 640;

        public int requestedWidth
        {
            get { return _requestedWidth; }
            set
            {
                int _value = (int)Mathf.Clamp(value, 0f, float.MaxValue);
                if (_requestedWidth != _value)
                {
                    _requestedWidth = _value;
                    if (hasInitDone)
                        Initialize();
                }
            }
        }

        /// <summary>
        /// Set the height of camera.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("requestedHeight"), TooltipAttribute("Set the height of camera.")]
        protected int _requestedHeight = 480;

        public int requestedHeight
        {
            get { return _requestedHeight; }
            set
            {
                int _value = (int)Mathf.Clamp(value, 0f, float.MaxValue);
                if (_requestedHeight != _value)
                {
                    _requestedHeight = _value;
                    if (hasInitDone)
                        Initialize();
                }
            }
        }

        /// <summary>
        /// Set whether to use the front facing camera.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("requestedIsFrontFacing"), TooltipAttribute("Set whether to use the front facing camera.")]
        protected bool _requestedIsFrontFacing = true;

        public bool requestedIsFrontFacing
        {
            get { return _requestedIsFrontFacing; }
            set
            {
                if (_requestedIsFrontFacing != value)
                {
                    _requestedIsFrontFacing = value;
                    if (hasInitDone)
                        Initialize(_requestedIsFrontFacing, requestedFPS, rotate90Degree);
                }
            }
        }

        /// <summary>
        /// Set the frame rate of camera.
        /// </summary>
        //  [SerializeField, FormerlySerializedAs("requestedFPS"), TooltipAttribute("Set the frame rate of camera.")]
        protected float _requestedFPS = 30f;

        public float requestedFPS
        {
            get { return _requestedFPS; }
            set
            {
                float _value = Mathf.Clamp(value, -1f, float.MaxValue);
                if (_requestedFPS != _value)
                {
                    _requestedFPS = _value;
                    if (hasInitDone)
                    {
                        bool isPlaying = IsPlaying();
                        Stop();
                        webCamTexture.requestedFPS = _requestedFPS;
                        if (isPlaying)
                            Play();
                    }
                }
            }
        }

        /// <summary>
        /// Sets whether to rotate camera frame 90 degrees. (clockwise)
        /// </summary>
        //[SerializeField, FormerlySerializedAs("rotate90Degree"), TooltipAttribute("Sets whether to rotate camera frame 90 degrees. (clockwise)")]
        protected bool _rotate90Degree = false;

        public bool rotate90Degree
        {
            get { return _rotate90Degree; }
            set
            {
                if (_rotate90Degree != value)
                {
                    _rotate90Degree = value;
                    if (hasInitDone)
                        Initialize();
                }
            }
        }

        /// <summary>
        /// Determines if flips vertically.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("flipVertical"), TooltipAttribute("Determines if flips vertically.")]
        protected bool _flipVertical = false;

        public bool flipVertical
        {
            get { return _flipVertical; }
            set { _flipVertical = value; }
        }

        /// <summary>
        /// Determines if flips horizontal.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("flipHorizontal"), TooltipAttribute("Determines if flips horizontal.")]
        protected bool _flipHorizontal = true;

        public bool flipHorizontal
        {
            get { return _flipHorizontal; }
            set { _flipHorizontal = value; }
        }

        /// <summary>
        /// Select the output color format.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("outputColorFormat"), TooltipAttribute("Select the output color format.")]
        protected ColorFormat _outputColorFormat = ColorFormat.RGBA;

        public ColorFormat outputColorFormat
        {
            get { return _outputColorFormat; }
            set
            {
                if (_outputColorFormat != value)
                {
                    _outputColorFormat = value;
                    if (hasInitDone)
                        Initialize();
                }
            }
        }

        /// <summary>
        /// The number of frames before the initialization process times out.
        /// </summary>
        //[SerializeField, FormerlySerializedAs("timeoutFrameCount"), TooltipAttribute("The number of frames before the initialization process times out.")]
        protected int _timeoutFrameCount = 300;

        public int timeoutFrameCount
        {
            get { return _timeoutFrameCount; }
            set { _timeoutFrameCount = (int)Mathf.Clamp(value, 0f, float.MaxValue); }
        }


        /// <summary>
        /// The active WebcamTexture.
        /// </summary>
        protected WebCamTexture webCamTexture;

        /// <summary>
        /// The active WebcamDevice.
        /// </summary>
        protected WebCamDevice webCamDevice;

        /// <summary>
        /// The frame mat.
        /// </summary>
        protected Mat frameMat;

        /// <summary>
        /// The base mat.
        /// </summary>
        protected Mat baseMat;

        /// <summary>
        /// The rotated frame mat
        /// </summary>
        protected Mat rotatedFrameMat;

        protected Mat clonededFrameMat;

        /// <summary>
        /// The buffer colors.
        /// </summary>
        protected Color32[] colors;

        /// <summary>
        /// The base color format.
        /// </summary>
        protected ColorFormat baseColorFormat = ColorFormat.RGBA;

        /// <summary>
        /// Indicates whether this instance is waiting for initialization to complete.
        /// </summary>
        protected bool isInitWaiting = false;

        /// <summary>
        /// Indicates whether this instance has been initialized.
        /// </summary>
        protected bool hasInitDone = false;

        /// <summary>
        /// The initialization coroutine.
        /// </summary>
        protected IEnumerator initCoroutine;

        /// <summary>
        /// The orientation of the screen.
        /// </summary>
        protected ScreenOrientation screenOrientation;

        /// <summary>
        /// The width of the screen.
        /// </summary>
        protected int screenWidth;

        /// <summary>
        /// The height of the screen.
        /// </summary>
        protected int screenHeight;

        /// <summary>
        /// Indicates whether this instance avoids the front camera low light issue that occurs in only some Android devices (e.g. Google Pixel, Pixel2).
        /// Sets compulsorily the requestedFPS parameter to 15 (only when using the front camera), to avoid the problem of the WebCamTexture image becoming low light.
        /// https://forum.unity.com/threads/android-webcamtexture-in-low-light-only-some-models.520656/
        /// https://forum.unity.com/threads/released-opencv-for-unity.277080/page-33#post-3445178
        /// </summary>
        bool avoidAndroidFrontCameraLowLightIssue = false;

        public enum ColorFormat : int
        {
            GRAY = 0,
            RGB,
            BGR,
            RGBA,
            BGRA,
        }

        public enum ErrorCode : int
        {
            UNKNOWN = 0,
            CAMERA_DEVICE_NOT_EXIST,
            CAMERA_PERMISSION_DENIED,
            TIMEOUT,
        }



        protected void OnValidate()
        {
            _requestedWidth = (int)Mathf.Clamp(_requestedWidth, 0f, float.MaxValue);
            _requestedHeight = (int)Mathf.Clamp(_requestedHeight, 0f, float.MaxValue);
            _requestedFPS = Mathf.Clamp(_requestedFPS, -1f, float.MaxValue);
            _timeoutFrameCount = (int)Mathf.Clamp(_timeoutFrameCount, 0f, float.MaxValue);
        }

#if !UNITY_EDITOR && !UNITY_ANDROID
          protected bool isScreenSizeChangeWaiting = false;
#endif

        protected void Update()
        {
            if (hasInitDone)
            {
                // Catch the orientation change of the screen and correct the mat image to the correct direction.
                if (screenOrientation != Screen.orientation)
                {

#if !UNITY_EDITOR && !UNITY_ANDROID
                      // Wait one frame until the Screen.width/Screen.height property changes.
                      if (!isScreenSizeChangeWaiting)
                      {
                          isScreenSizeChangeWaiting = true;
                          return;
                      }
                      isScreenSizeChangeWaiting = false;
#endif

                    //if (onDisposed != null)
                    //    onDisposed.Invoke();
                    EventsManagerLib.CALL_ndi_dispose_evnt();

                    if (frameMat != null)
                    {
                        frameMat.Dispose();
                        frameMat = null;
                    }
                    if (baseMat != null)
                    {
                        baseMat.Dispose();
                        baseMat = null;
                    }
                    if (rotatedFrameMat != null)
                    {
                        rotatedFrameMat.Dispose();
                        rotatedFrameMat = null;
                    }
                    if (clonededFrameMat != null)
                    {
                        clonededFrameMat.Dispose();
                        clonededFrameMat = null;
                    }
                    //NABIL

                    baseMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4, new Scalar(0, 0, 0, 255));

                    if (baseColorFormat == outputColorFormat)
                    {
                        frameMat = baseMat;
                    }
                    else
                    {
                        frameMat = new Mat(baseMat.rows(), baseMat.cols(), CvType.CV_8UC(Channels(outputColorFormat)), new Scalar(0, 0, 0, 255));
                    }

                    clonededFrameMat = frameMat.clone();

                    screenOrientation = Screen.orientation;
                    screenWidth = Screen.width;
                    screenHeight = Screen.height;

                    bool isRotatedFrame = false;
#if !UNITY_EDITOR && !(UNITY_STANDALONE || UNITY_WEBGL)
                      if (screenOrientation == ScreenOrientation.Portrait || screenOrientation == ScreenOrientation.PortraitUpsideDown)
                      {
                          if (!rotate90Degree)
                              isRotatedFrame = true;
                      }
                      else if (rotate90Degree)
                      {
                          isRotatedFrame = true;
                      }
#else
                    if (rotate90Degree)
                        isRotatedFrame = true;
#endif
                    if (isRotatedFrame)
                        rotatedFrameMat = new Mat(frameMat.cols(), frameMat.rows(), CvType.CV_8UC(Channels(outputColorFormat)), new Scalar(0, 0, 0, 255));

                    //if (onInitialized != null)
                    //    onInitialized.Invoke();
                    Debug.Log("calling init event");
                    EventsManagerLib.CALL_NDI_StartedStreaming(webCamTexture.width, webCamTexture.height);
                    Debug.Log("called init event");
                }
            }
        }

        protected IEnumerator OnApplicationFocus(bool hasFocus)
        {
#if (UNITY_IOS && UNITY_2018_1_OR_NEWER) || (UNITY_ANDROID && UNITY_2018_3_OR_NEWER)
              yield return null;

              if (isUserRequestingPermission && hasFocus)
                  isUserRequestingPermission = false;
#endif
            yield break;
        }

        /// <summary>
        /// Raises the destroy event.
        /// </summary>
        protected void OnDestroy()
        {
            Dispose();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }



            initCoroutine = _Initialize();
            StartCoroutine(initCoroutine);
        }


        public void Initialize(int requestedWidth, int requestedHeight)
        {
            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }

            _requestedWidth = requestedWidth;
            _requestedHeight = requestedHeight;


            initCoroutine = _Initialize();
            StartCoroutine(initCoroutine);
        }


        public void Initialize(string deviceName, int requestedWidth, int requestedHeight)
        {
            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }

            _requestedDeviceName = deviceName;
            _requestedWidth = requestedWidth;
            _requestedHeight = requestedHeight;


            initCoroutine = _Initialize();
            StartCoroutine(initCoroutine);
        }

        public void Initialize(bool requestedIsFrontFacing, float requestedFPS = 30f, bool rotate90Degree = false)
        {
            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }

            _requestedDeviceName = null;
            _requestedIsFrontFacing = requestedIsFrontFacing;
            _requestedFPS = requestedFPS;
            _rotate90Degree = rotate90Degree;


            initCoroutine = _Initialize();
            StartCoroutine(initCoroutine);
        }

        public void Initialize(string deviceName, int requestedWidth, int requestedHeight, bool requestedIsFrontFacing = false, float requestedFPS = 30f, bool rotate90Degree = false)
        {
            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }

            _requestedDeviceName = deviceName;
            _requestedWidth = requestedWidth;
            _requestedHeight = requestedHeight;
            _requestedIsFrontFacing = requestedIsFrontFacing;
            _requestedFPS = requestedFPS;
            _rotate90Degree = rotate90Degree;


            initCoroutine = _Initialize();
            StartCoroutine(initCoroutine);
        }

        protected IEnumerator _Initialize()
        {
            if (hasInitDone)
            {
                ReleaseResources();


                EventsManagerLib.CALL_ndi_dispose_evnt();
            }

            isInitWaiting = true;



            float requestedFPS = this.requestedFPS;


            var devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++)
                Debug.Log(devices[i].name);

            if (!String.IsNullOrEmpty(requestedDeviceName))
            {
                int requestedDeviceIndex = -1;
                if (Int32.TryParse(requestedDeviceName, out requestedDeviceIndex))
                {
                    if (requestedDeviceIndex >= 0 && requestedDeviceIndex < devices.Length)
                    {
                        webCamDevice = devices[requestedDeviceIndex];

                        if (avoidAndroidFrontCameraLowLightIssue && webCamDevice.isFrontFacing == true)
                            requestedFPS = 15f;

                        if (requestedFPS < 0)
                        {
                            webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight);
                        }
                        else
                        {
                            webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, (int)requestedFPS);
                        }
                    }
                }
                else
                {
                    for (int cameraIndex = 0; cameraIndex < devices.Length; cameraIndex++)
                    {
                        if (devices[cameraIndex].name == requestedDeviceName)
                        {
                            webCamDevice = devices[cameraIndex];

                            if (avoidAndroidFrontCameraLowLightIssue && webCamDevice.isFrontFacing == true)
                                requestedFPS = 15f;

                            if (requestedFPS < 0)
                            {
                                webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight);
                            }
                            else
                            {
                                webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, (int)requestedFPS);
                            }
                            break;
                        }
                    }
                }
                if (webCamTexture == null)
                    Debug.Log("Cannot find camera device " + requestedDeviceName + ".");
            }

            if (webCamTexture == null)
            {
                // Checks how many and which cameras are available on the device
                for (int cameraIndex = 0; cameraIndex < devices.Length; cameraIndex++)
                {
#if UNITY_2018_3_OR_NEWER
                    if (devices[cameraIndex].kind != WebCamKind.ColorAndDepth && devices[cameraIndex].isFrontFacing == requestedIsFrontFacing)
#else
                      if (devices[cameraIndex].isFrontFacing == requestedIsFrontFacing)
#endif
                    {
                        webCamDevice = devices[cameraIndex];

                        if (avoidAndroidFrontCameraLowLightIssue && webCamDevice.isFrontFacing == true)
                            requestedFPS = 15f;

                        if (requestedFPS < 0)
                        {
                            webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight);
                        }
                        else
                        {
                            webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, (int)requestedFPS);
                        }
                        break;
                    }
                }
            }

            if (webCamTexture == null)
            {
                if (devices.Length > 0)
                {
                    webCamDevice = devices[0];

                    if (avoidAndroidFrontCameraLowLightIssue && webCamDevice.isFrontFacing == true)
                        requestedFPS = 15f;

                    if (requestedFPS < 0)
                    {
                        webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight);
                    }
                    else
                    {
                        webCamTexture = new WebCamTexture(webCamDevice.name, requestedWidth, requestedHeight, (int)requestedFPS);
                    }
                }
                else
                {
                    isInitWaiting = false;
                    initCoroutine = null;


                    EventsManagerLib.CALL_ndi_Error_evnt(1);//cam cant be found

                    yield break;
                }
            }

            // Starts the camera
            webCamTexture.Play();

            int initFrameCount = 0;
            bool isTimeout = false;

            while (true)
            {
                if (initFrameCount > timeoutFrameCount)
                {
                    isTimeout = true;
                    break;
                }
                else if (webCamTexture.didUpdateThisFrame)
                {
                    Debug.Log("WebCamTextureToMatHelper:: " + "devicename:" + webCamTexture.deviceName + " name:" + webCamTexture.name + " width:" + webCamTexture.width + " height:" + webCamTexture.height + " fps:" + webCamTexture.requestedFPS
                    + " videoRotationAngle:" + webCamTexture.videoRotationAngle + " videoVerticallyMirrored:" + webCamTexture.videoVerticallyMirrored + " isFrongFacing:" + webCamDevice.isFrontFacing);

                    if (colors == null || colors.Length != webCamTexture.width * webCamTexture.height)
                        colors = new Color32[webCamTexture.width * webCamTexture.height];

                    baseMat = new Mat(webCamTexture.height, webCamTexture.width, CvType.CV_8UC4);

                    if (baseColorFormat == outputColorFormat)
                    {
                        frameMat = baseMat;
                    }
                    else
                    {
                        frameMat = new Mat(baseMat.rows(), baseMat.cols(), CvType.CV_8UC(Channels(outputColorFormat)), new Scalar(0, 0, 0, 255));
                    }

                    screenOrientation = Screen.orientation;
                    screenWidth = Screen.width;
                    screenHeight = Screen.height;

                    bool isRotatedFrame = false;
#if !UNITY_EDITOR && !(UNITY_STANDALONE || UNITY_WEBGL)
                      if (screenOrientation == ScreenOrientation.Portrait || screenOrientation == ScreenOrientation.PortraitUpsideDown)
                      {
                          if (!rotate90Degree)
                              isRotatedFrame = true;
                      }
                      else if (rotate90Degree)
                      {
                          isRotatedFrame = true;
                      }
#else
                    if (rotate90Degree)
                        isRotatedFrame = true;
#endif
                    if (isRotatedFrame)
                        rotatedFrameMat = new Mat(frameMat.cols(), frameMat.rows(), CvType.CV_8UC(Channels(outputColorFormat)), new Scalar(0, 0, 0, 255));

                    isInitWaiting = false;
                    hasInitDone = true;
                    Debug.Log("image found and feed ready " + webCamTexture.width + "x" + webCamTexture.height);



                    Debug.Log("callinginit");
                    EventsManagerLib.CALL_NDI_StartedStreaming(webCamTexture.width, webCamTexture.height);
                    Debug.Log("calledinit");

                    initCoroutine = null;


                    break;
                }
                else
                {
                    initFrameCount++;
                    yield return null;
                }
            }

            if (isTimeout)
            {
                webCamTexture.Stop();
                webCamTexture = null;
                isInitWaiting = false;
                initCoroutine = null;



                EventsManagerLib.CALL_ndi_Error_evnt(3);//timed out

            }
        }


        protected IEnumerator hasUserAuthorizedCameraPermission()
        {
#if UNITY_IOS && UNITY_2018_1_OR_NEWER
              UserAuthorization mode = UserAuthorization.WebCam;
              if (!Application.HasUserAuthorization(mode))
              {
                  yield return RequestUserAuthorization(mode);
              }
              yield return Application.HasUserAuthorization(mode);
#elif UNITY_ANDROID && UNITY_2018_3_OR_NEWER
              string permission = UnityEngine.Android.Permission.Camera;
              if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission))
              {
                  yield return RequestUserPermission(permission);
              }
              yield return UnityEngine.Android.Permission.HasUserAuthorizedPermission(permission);
#else
            yield return true;
#endif
        }

#if (UNITY_IOS && UNITY_2018_1_OR_NEWER) || (UNITY_ANDROID && UNITY_2018_3_OR_NEWER)
          protected bool isUserRequestingPermission;
#endif

#if UNITY_IOS && UNITY_2018_1_OR_NEWER
          protected  IEnumerator RequestUserAuthorization(UserAuthorization mode)
          {
              isUserRequestingPermission = true;
              yield return Application.RequestUserAuthorization(mode);

              float timeElapsed = 0;
              while (isUserRequestingPermission)
              {
                  if (timeElapsed > 0.25f)
                  {
                      isUserRequestingPermission = false;
                      yield break;
                  }
                  timeElapsed += Time.deltaTime;

                  yield return null;
              }
              yield break;
          }
#elif UNITY_ANDROID && UNITY_2018_3_OR_NEWER
          protected  IEnumerator RequestUserPermission(string permission)
          {
              isUserRequestingPermission = true;
              UnityEngine.Android.Permission.RequestUserPermission(permission);

              float timeElapsed = 0;
              while (isUserRequestingPermission)
              {
                  if (timeElapsed > 0.25f)
                  {
                      isUserRequestingPermission = false;
                      yield break;
                  }
                  timeElapsed += Time.deltaTime;

                  yield return null;
              }
              yield break;
          }
#endif


        public bool IsInitialized()
        {
            return hasInitDone;
        }


        public void Play()
        {
            if (hasInitDone)
                webCamTexture.Play();
        }

        public void Pause()
        {
            if (hasInitDone)
                webCamTexture.Pause();
        }

        public void Stop()
        {
            if (hasInitDone)
                webCamTexture.Stop();
        }


        public bool IsPlaying()
        {
            return hasInitDone ? webCamTexture.isPlaying : false;
        }


        public bool IsFrontFacing()
        {
            return hasInitDone ? webCamDevice.isFrontFacing : false;
        }


        public string GetDeviceName()
        {
            return hasInitDone ? webCamTexture.deviceName : "";
        }


        public int GetWidth()
        {
            if (!hasInitDone)
                return -1;
            return (rotatedFrameMat != null) ? frameMat.height() : frameMat.width();
        }


        public int GetHeight()
        {
            if (!hasInitDone)
                return -1;
            return (rotatedFrameMat != null) ? frameMat.width() : frameMat.height();
        }


        public float GetFPS()
        {
            return hasInitDone ? webCamTexture.requestedFPS : -1f;
        }


        public WebCamTexture GetWebCamTexture()
        {
            return hasInitDone ? webCamTexture : null;
        }


        public WebCamDevice GetWebCamDevice()
        {
            return webCamDevice;
        }


        public Matrix4x4 GetCameraToWorldMatrix()
        {
            return Camera.main.cameraToWorldMatrix;
        }


        public Matrix4x4 GetProjectionMatrix()
        {
            return Camera.main.projectionMatrix;
        }


        public bool DidUpdateThisFrame()
        {
            if (!hasInitDone)
                return false;

            return webCamTexture.didUpdateThisFrame;
        }


        public Mat GetMat()
        {
            if (!hasInitDone || !webCamTexture.isPlaying)
            {
                return (rotatedFrameMat != null) ? rotatedFrameMat : frameMat;
            }

            if (baseColorFormat == outputColorFormat)
            {
                Utils.webCamTextureToMat(webCamTexture, frameMat, colors, false);
            }
            else
            {
                Utils.webCamTextureToMat(webCamTexture, baseMat, colors, false);
                Imgproc.cvtColor(baseMat, frameMat, ColorConversionCodes(baseColorFormat, outputColorFormat));
            }

#if !UNITY_EDITOR && !(UNITY_STANDALONE || UNITY_WEBGL)
              if (rotatedFrameMat != null)
              {
                  if (screenOrientation == ScreenOrientation.Portrait || screenOrientation == ScreenOrientation.PortraitUpsideDown)
                  {
                      // (Orientation is Portrait, rotate90Degree is false)
                      if (webCamDevice.isFrontFacing)
                      {
                          FlipMat(frameMat, !flipHorizontal, !flipVertical);
                      }
                      else
                      {
                          FlipMat(frameMat, flipHorizontal, flipVertical);
                      }
                  }
                  else
                  {
                      // (Orientation is Landscape, rotate90Degrees=true)
                      FlipMat(frameMat, flipVertical, flipHorizontal);
                  }
                  Core.rotate(frameMat, rotatedFrameMat, Core.ROTATE_90_CLOCKWISE);
                  return rotatedFrameMat;
              }
              else
              {
                  if (screenOrientation == ScreenOrientation.Portrait || screenOrientation == ScreenOrientation.PortraitUpsideDown)
                  {
                      // (Orientation is Portrait, rotate90Degree is ture)
                      if (webCamDevice.isFrontFacing)
                      {
                          FlipMat(frameMat, flipHorizontal, flipVertical);
                      }
                      else
                      {
                          FlipMat(frameMat, !flipHorizontal, !flipVertical);
                      }
                  }
                  else
                  {
                      // (Orientation is Landscape, rotate90Degree is false)
                      FlipMat(frameMat, flipVertical, flipHorizontal);
                  }
                  return frameMat;
              }
#else
            FlipMat(frameMat, flipVertical, flipHorizontal);
            if (rotatedFrameMat != null)
            {
                Core.rotate(frameMat, rotatedFrameMat, Core.ROTATE_90_CLOCKWISE);
                return rotatedFrameMat;
            }
            else
            {
                return frameMat;
            }
#endif
        }

        protected void FlipMat(Mat mat, bool flipVertical, bool flipHorizontal)
        {
            //Since the order of pixels of WebCamTexture and Mat is opposite, the initial value of flipCode is set to 0 (flipVertical).
            int flipCode = 0;

            if (webCamDevice.isFrontFacing)
            {
                if (webCamTexture.videoRotationAngle == 0 || webCamTexture.videoRotationAngle == 90)
                {
                    flipCode = -1;
                }
                else if (webCamTexture.videoRotationAngle == 180 || webCamTexture.videoRotationAngle == 270)
                {
                    flipCode = int.MinValue;
                }
            }
            else
            {
                if (webCamTexture.videoRotationAngle == 180 || webCamTexture.videoRotationAngle == 270)
                {
                    flipCode = 1;
                }
            }

            if (flipVertical)
            {
                if (flipCode == int.MinValue)
                {
                    flipCode = 0;
                }
                else if (flipCode == 0)
                {
                    flipCode = int.MinValue;
                }
                else if (flipCode == 1)
                {
                    flipCode = -1;
                }
                else if (flipCode == -1)
                {
                    flipCode = 1;
                }
            }

            if (flipHorizontal)
            {
                if (flipCode == int.MinValue)
                {
                    flipCode = 1;
                }
                else if (flipCode == 0)
                {
                    flipCode = -1;
                }
                else if (flipCode == 1)
                {
                    flipCode = int.MinValue;
                }
                else if (flipCode == -1)
                {
                    flipCode = 0;
                }
            }

            if (flipCode > int.MinValue)
            {
                Core.flip(mat, mat, flipCode);
            }
        }

        protected int Channels(ColorFormat type)
        {
            switch (type)
            {
                case ColorFormat.GRAY:
                    return 1;
                case ColorFormat.RGB:
                case ColorFormat.BGR:
                    return 3;
                case ColorFormat.RGBA:
                case ColorFormat.BGRA:
                    return 4;
                default:
                    return 4;
            }
        }
        protected int ColorConversionCodes(ColorFormat srcType, ColorFormat dstType)
        {
            if (srcType == ColorFormat.GRAY)
            {
                if (dstType == ColorFormat.RGB) return Imgproc.COLOR_GRAY2RGB;
                else if (dstType == ColorFormat.BGR) return Imgproc.COLOR_GRAY2BGR;
                else if (dstType == ColorFormat.RGBA) return Imgproc.COLOR_GRAY2RGBA;
                else if (dstType == ColorFormat.BGRA) return Imgproc.COLOR_GRAY2BGRA;
            }
            else if (srcType == ColorFormat.RGB)
            {
                if (dstType == ColorFormat.GRAY) return Imgproc.COLOR_RGB2GRAY;
                else if (dstType == ColorFormat.BGR) return Imgproc.COLOR_RGB2BGR;
                else if (dstType == ColorFormat.RGBA) return Imgproc.COLOR_RGB2RGBA;
                else if (dstType == ColorFormat.BGRA) return Imgproc.COLOR_RGB2BGRA;
            }
            else if (srcType == ColorFormat.BGR)
            {
                if (dstType == ColorFormat.GRAY) return Imgproc.COLOR_BGR2GRAY;
                else if (dstType == ColorFormat.RGB) return Imgproc.COLOR_BGR2RGB;
                else if (dstType == ColorFormat.RGBA) return Imgproc.COLOR_BGR2RGBA;
                else if (dstType == ColorFormat.BGRA) return Imgproc.COLOR_BGR2BGRA;
            }
            else if (srcType == ColorFormat.RGBA)
            {
                if (dstType == ColorFormat.GRAY) return Imgproc.COLOR_RGBA2GRAY;
                else if (dstType == ColorFormat.RGB) return Imgproc.COLOR_RGBA2RGB;
                else if (dstType == ColorFormat.BGR) return Imgproc.COLOR_RGBA2BGR;
                else if (dstType == ColorFormat.BGRA) return Imgproc.COLOR_RGBA2BGRA;
            }
            else if (srcType == ColorFormat.BGRA)
            {
                if (dstType == ColorFormat.GRAY) return Imgproc.COLOR_BGRA2GRAY;
                else if (dstType == ColorFormat.RGB) return Imgproc.COLOR_BGRA2RGB;
                else if (dstType == ColorFormat.BGR) return Imgproc.COLOR_BGRA2BGR;
                else if (dstType == ColorFormat.RGBA) return Imgproc.COLOR_BGRA2RGBA;
            }

            return -1;
        }


        public Color32[] GetBufferColors()
        {
            return colors;
        }


        protected void CancelInitCoroutine()
        {
            if (initCoroutine != null)
            {
                StopCoroutine(initCoroutine);
                ((IDisposable)initCoroutine).Dispose();
                initCoroutine = null;
            }
        }


        protected void ReleaseResources()
        {
            isInitWaiting = false;
            hasInitDone = false;

            if (webCamTexture != null)
            {
                webCamTexture.Stop();
                WebCamTexture.Destroy(webCamTexture);
                webCamTexture = null;
            }
            if (frameMat != null)
            {
                frameMat.Dispose();
                frameMat = null;
            }
            if (baseMat != null)
            {
                baseMat.Dispose();
                baseMat = null;
            }
            if (rotatedFrameMat != null)
            {
                rotatedFrameMat.Dispose();
                rotatedFrameMat = null;
            }
        }


        public void Dispose()
        {
            if (colors != null)
                colors = null;

            if (isInitWaiting)
            {
                CancelInitCoroutine();
                ReleaseResources();
            }
            else if (hasInitDone)
            {
                ReleaseResources();

                EventsManagerLib.CALL_ndi_dispose_evnt();

            }
        }


    }


#endif
#endif
}