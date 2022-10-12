#define USEBOOLS
//using OpenCVForUnity.CoreModule;
 using OpenCVForUnity.UnityUtils;
using OpenCVForUnity.CoreModule;
using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
 using static OpenCVForUnity.UnityUtils.Helper.VideoCaptureToMatHelper;
using IntPtr = System.IntPtr;
using Marshal = System.Runtime.InteropServices.Marshal;

namespace Klak.Ndi {

[ExecuteInEditMode]
    [System.Runtime.InteropServices.Guid("0BE220BB-034F-439E-B03D-6DEF5912B85B")]
    public sealed partial class NdiReceiver : MonoBehaviour
{
        Mat s;
         
       // bool _NdiReceiverUpdatedThisFrame = false;
        public int frameWidth { get; set; }
        public int frameHeight { get; set; }
      //  public bool didNDIUpdateThisFrameOnce { get => _NdiReceiverUpdatedThisFrame; }

      //  bool _IsPlaying = false;
      //  public bool IsPlaying { get => _IsPlaying; }




        #region Receiver objects

        Interop.Recv _recv;
    FormatConverter _converter;
    MaterialPropertyBlock _override;

    void PrepareReceiverObjects()
    {
        if (_recv == null) _recv = RecvHelper.TryCreateRecv(ndiName);
        if (_converter == null) _converter = new FormatConverter(_resources);
        if (_override == null) _override = new MaterialPropertyBlock();
    }

    void ReleaseReceiverObjects()
    {
            jumpstartedonce = false;
            _recv?.Dispose();
        _recv = null;

        _converter?.Dispose();
        _converter = null;

        // We don't dispose _override because it's reusable.
    }

        #endregion

        #region Receiver implementation

       
   public RenderTexture TryReceiveFrame()
    {
        PrepareReceiverObjects();
            if (_recv == null) {
               // _IsPlaying = false;
                return null;
            }

        // Video frame capturing
        var frameOrNull = RecvHelper.TryCaptureVideoFrame(_recv);
            if (frameOrNull == null) {
                //Debug.Log("noimage");
               // _NdiReceiverUpdatedThisFrame = false;
               // _IsPlaying = false;
                return null; 
            }
        var frame = (Interop.VideoFrame)frameOrNull;
//*************************************************************************
            frameWidth = frame.Width;
            frameHeight = frame.Height;
//*************************************************************************
            // Pixel format conversion
            var rt = _converter.Decode
          (frame.Width, frame.Height, Util.HasAlpha(frame.FourCC), frame.Data);
            if (rt == null) {
                Debug.Log(" YOOO !!! ndireceiver is maybe stopepd?");
            }
        // Metadata retrieval
        if (frame.Metadata != IntPtr.Zero)
            metadata = Marshal.PtrToStringAnsi(frame.Metadata);
        else
            metadata = null;

        // Video frame release
        _recv.FreeVideoFrame(frame);
       // _NdiReceiverUpdatedThisFrame = true;
         //   _IsPlaying = true;
            // Debug.Log("IMAGE!");

            return rt;
    }

    #endregion

    #region Component state controller

    internal void Restart() => ReleaseReceiverObjects();

        #endregion

        #region MonoBehaviour implementation
        bool jumpstartedonce = false;
        public bool HackyJumpStart() {

            if (jumpstartedonce)
            {
                return true;
            }
            else
            {
                var RT = TryReceiveFrame();
                if (RT == null)
                {
                    return false;
                }
                else
                {
                    jumpstartedonce = true;
                    return true;
                }
            }


           
        }

    public void HElpRunMePlz(RenderTexture rt) {
        if (AllowRendering)
        {

           //var rt = TryReceiveFrame();
            if (rt == null) return;


            // Material property override
            if (targetRenderer != null)
            {
                targetRenderer.GetPropertyBlock(_override);
                _override.SetTexture(targetMaterialProperty, rt);
                targetRenderer.SetPropertyBlock(_override);
            }

 
            if (AllowBlit)
            {
                if (targetTexture != null)
                        Graphics.Blit(rt, targetTexture);
                        //Graphics.Blit(rt, targetTexture, new Vector2(1, 1), new Vector2(0.5f, 0.5f));
            }
        }
    }
    void OnDisable() => ReleaseReceiverObjects();
        /*

                void Update()
                {
        #if USEBOOLS

                    if (AllowRendering)
                    {
        #endif
                        var rt = TryReceiveFrame();
                        if (rt == null) return;


                            // Material property override
                            if (targetRenderer != null)
                            {
                                targetRenderer.GetPropertyBlock(_override);
                                _override.SetTexture(targetMaterialProperty, rt);
                                targetRenderer.SetPropertyBlock(_override);
                            }

        #if USEBOOLS
                    if (AllowBlit)
                    {
        #endif
                            // External texture update
                            if (targetTexture != null)
                                Graphics.Blit(rt, targetTexture);
                                //Graphics.Blit(rt, targetTexture, new Vector2(1, 1), new Vector2(0.5f, 0.5f));
        #if USEBOOLS

                }
            }
        #endif


        }
                */
        #endregion





        void Update()
        {
#if USEBOOLS

            if (AllowRendering)
            {
#endif
            var rt = TryReceiveFrame();
            if (rt == null) return;


            // Material property override
            if (targetRenderer != null)
            {
                targetRenderer.GetPropertyBlock(_override);
                _override.SetTexture(targetMaterialProperty, rt);
                targetRenderer.SetPropertyBlock(_override);
            }

#if USEBOOLS
            if (AllowBlit)
            {
#endif
            // External texture update
            if (targetTexture != null)
                Graphics.Blit(rt, targetTexture);
            //Graphics.Blit(rt, targetTexture, new Vector2(1, 1), new Vector2(0.5f, 0.5f));
#if USEBOOLS

        }
    }
#endif


        }




    }

} // namespace Klak.Ndi
