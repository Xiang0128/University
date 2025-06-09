using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Newtonsoft.Json;

public class UDPSocketReceiver : MonoBehaviour
{
    [System.Serializable]
    public class Landmark
    {
        public float x;
        public float y;
        public float z;
    }

    [System.Serializable]
    public class LandmarksData
    {
        public Dictionary<int, Landmark> pose;       // Pose landmarks (11, 12, 13, 14)
        public Dictionary<int, Landmark> left_hand;  // Left hand landmarks (0 to 20)
        public Dictionary<int, Landmark> right_hand; // Right hand landmarks (0 to 20)
    }

    public Transform leftShoulderTransform;
    public Transform leftElbowTransform;
    public Transform leftWristTransform;

    public Transform rightShoulderTransform;
    public Transform rightElbowTransform;
    public Transform rightWristTransform;

    public Transform leftThumb1Transform, leftThumb2Transform, leftThumb3Transform;
    public Transform leftIndex1Transform, leftIndex2Transform, leftIndex3Transform;
    public Transform leftMiddle1Transform, leftMiddle2Transform, leftMiddle3Transform;
    public Transform leftRing1Transform, leftRing2Transform, leftRing3Transform;
    public Transform leftPinky1Transform, leftPinky2Transform, leftPinky3Transform;

    public Transform rightThumb1Transform, rightThumb2Transform, rightThumb3Transform;
    public Transform rightIndex1Transform, rightIndex2Transform, rightIndex3Transform;
    public Transform rightMiddle1Transform, rightMiddle2Transform, rightMiddle3Transform;
    public Transform rightRing1Transform, rightRing2Transform, rightRing3Transform;
    public Transform rightPinky1Transform, rightPinky2Transform, rightPinky3Transform;

    private UdpClient udpClient;
    private Thread receiveThread;
    private LandmarksData landmarksData;

    void Start()
    {
        udpClient = new UdpClient(5052);  // Adjust port if necessary
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }

    void ReceiveData()
    {
        while (true)
        {
            try
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 5052);
                byte[] data = udpClient.Receive(ref remoteEndPoint);
                string jsonString = Encoding.UTF8.GetString(data);
                landmarksData = JsonConvert.DeserializeObject<LandmarksData>(jsonString);
            }
            catch (System.Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }
    }

    void Update()
    {
        if (landmarksData != null)
        {
            ApplyArmRotations();
            ApplyHandRotations();
        }
    }

    private void ApplyRotation(Vector3 start, Vector3 end, Transform joint, bool invertX = false, bool invertY = true, bool invertZ = false)
    {
        // Calculate direction
        Vector3 direction = end - start;

        // Optionally invert specific axes on the direction vector
        direction = new Vector3(
            invertX ? -direction.x : direction.x,
            invertY ? -direction.y : direction.y,
            invertZ ? -direction.z : direction.z
        );

        // Compute rotation using the adjusted direction
        Quaternion rotation = Quaternion.LookRotation(direction);

        // Apply the rotation directly to the joint
        joint.rotation = rotation;
    }



    private void ApplyArmRotations()
    {
        // Left arm: Shoulder -> Elbow -> Wrist
        if (landmarksData.pose.ContainsKey(11) && landmarksData.pose.ContainsKey(13) && landmarksData.left_hand.ContainsKey(0))
        {
            Vector3 shoulderPos = new Vector3(landmarksData.pose[11].x, landmarksData.pose[11].y, landmarksData.pose[11].z);
            Vector3 elbowPos = new Vector3(landmarksData.pose[13].x, landmarksData.pose[13].y, landmarksData.pose[13].z);
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);

            ApplyRotation(shoulderPos, elbowPos, leftShoulderTransform);
            ApplyRotation(elbowPos, wristPos, leftElbowTransform);
        }

        // Right arm: Shoulder -> Elbow -> Wrist
        if (landmarksData.pose.ContainsKey(12) && landmarksData.pose.ContainsKey(14) && landmarksData.right_hand.ContainsKey(0))
        {
            Vector3 shoulderPos = new Vector3(landmarksData.pose[12].x, landmarksData.pose[12].y, landmarksData.pose[12].z);
            Vector3 elbowPos = new Vector3(landmarksData.pose[14].x, landmarksData.pose[14].y, landmarksData.pose[14].z);
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);

            ApplyRotation(shoulderPos, elbowPos, rightShoulderTransform);
            ApplyRotation(elbowPos, wristPos, rightElbowTransform);
        }
    }

    private void ApplyHandRotations()
    {
        // Left Thumb
        if (landmarksData.left_hand.ContainsKey(0) && landmarksData.left_hand.ContainsKey(1) &&
            landmarksData.left_hand.ContainsKey(2) && landmarksData.left_hand.ContainsKey(3) &&
            landmarksData.left_hand.ContainsKey(4))
        {
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);
            Vector3 thumb1Pos = new Vector3(landmarksData.left_hand[1].x, landmarksData.left_hand[1].y, landmarksData.left_hand[1].z);
            Vector3 thumb2Pos = new Vector3(landmarksData.left_hand[2].x, landmarksData.left_hand[2].y, landmarksData.left_hand[2].z);
            Vector3 thumb3Pos = new Vector3(landmarksData.left_hand[3].x, landmarksData.left_hand[3].y, landmarksData.left_hand[3].z);
            Vector3 thumb4Pos = new Vector3(landmarksData.left_hand[4].x, landmarksData.left_hand[4].y, landmarksData.left_hand[4].z);

            ApplyRotation(thumb1Pos, thumb2Pos, leftThumb1Transform);
            ApplyRotation(thumb2Pos, thumb3Pos, leftThumb2Transform);
            ApplyRotation(thumb3Pos, thumb4Pos, leftThumb3Transform);

        }

        // Left Index Finger
        if (landmarksData.left_hand.ContainsKey(5) && landmarksData.left_hand.ContainsKey(6) &&
            landmarksData.left_hand.ContainsKey(7) && landmarksData.left_hand.ContainsKey(8))
        {
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);
            Vector3 index1Pos = new Vector3(landmarksData.left_hand[5].x, landmarksData.left_hand[5].y, landmarksData.left_hand[5].z);
            Vector3 index2Pos = new Vector3(landmarksData.left_hand[6].x, landmarksData.left_hand[6].y, landmarksData.left_hand[6].z);
            Vector3 index3Pos = new Vector3(landmarksData.left_hand[7].x, landmarksData.left_hand[7].y, landmarksData.left_hand[7].z);
            Vector3 index4Pos = new Vector3(landmarksData.left_hand[8].x, landmarksData.left_hand[8].y, landmarksData.left_hand[8].z);

            ApplyRotation(index1Pos, index2Pos, leftIndex1Transform);
            ApplyRotation(index2Pos, index3Pos, leftIndex2Transform);
            ApplyRotation(index3Pos, index4Pos, leftIndex3Transform);

        }

        // Left Middle Finger
        if (landmarksData.left_hand.ContainsKey(9) && landmarksData.left_hand.ContainsKey(10) &&
            landmarksData.left_hand.ContainsKey(11) && landmarksData.left_hand.ContainsKey(12))
        {
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);
            Vector3 middle1Pos = new Vector3(landmarksData.left_hand[9].x, landmarksData.left_hand[9].y, landmarksData.left_hand[9].z);
            Vector3 middle2Pos = new Vector3(landmarksData.left_hand[10].x, landmarksData.left_hand[10].y, landmarksData.left_hand[10].z);
            Vector3 middle3Pos = new Vector3(landmarksData.left_hand[11].x, landmarksData.left_hand[11].y, landmarksData.left_hand[11].z);
            Vector3 middle4Pos = new Vector3(landmarksData.left_hand[12].x, landmarksData.left_hand[12].y, landmarksData.left_hand[12].z);

            ApplyRotation(middle1Pos, middle2Pos, leftMiddle1Transform);
            ApplyRotation(middle2Pos, middle3Pos, leftMiddle2Transform);
            ApplyRotation(middle3Pos, middle4Pos, leftMiddle3Transform);
        }

        // Left Ring Finger
        if (landmarksData.left_hand.ContainsKey(13) && landmarksData.left_hand.ContainsKey(14) &&
            landmarksData.left_hand.ContainsKey(15) && landmarksData.left_hand.ContainsKey(16))
        {
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);
            Vector3 ring1Pos = new Vector3(landmarksData.left_hand[13].x, landmarksData.left_hand[13].y, landmarksData.left_hand[13].z);
            Vector3 ring2Pos = new Vector3(landmarksData.left_hand[14].x, landmarksData.left_hand[14].y, landmarksData.left_hand[14].z);
            Vector3 ring3Pos = new Vector3(landmarksData.left_hand[15].x, landmarksData.left_hand[15].y, landmarksData.left_hand[15].z);
            Vector3 ring4Pos = new Vector3(landmarksData.left_hand[16].x, landmarksData.left_hand[16].y, landmarksData.left_hand[16].z);

            ApplyRotation(ring1Pos, ring2Pos, leftRing1Transform);
            ApplyRotation(ring2Pos, ring3Pos, leftRing2Transform);
            ApplyRotation(ring3Pos, ring4Pos, leftRing3Transform);
        }

        // Left Pinky Finger
        if (landmarksData.left_hand.ContainsKey(17) && landmarksData.left_hand.ContainsKey(18) &&
            landmarksData.left_hand.ContainsKey(19) && landmarksData.left_hand.ContainsKey(20))
        {
            Vector3 wristPos = new Vector3(landmarksData.left_hand[0].x, landmarksData.left_hand[0].y, landmarksData.left_hand[0].z);
            Vector3 pinky1Pos = new Vector3(landmarksData.left_hand[17].x, landmarksData.left_hand[17].y, landmarksData.left_hand[17].z);
            Vector3 pinky2Pos = new Vector3(landmarksData.left_hand[18].x, landmarksData.left_hand[18].y, landmarksData.left_hand[18].z);
            Vector3 pinky3Pos = new Vector3(landmarksData.left_hand[19].x, landmarksData.left_hand[19].y, landmarksData.left_hand[19].z);
            Vector3 pinky4Pos = new Vector3(landmarksData.left_hand[20].x, landmarksData.left_hand[20].y, landmarksData.left_hand[20].z);
            
            ApplyRotation(pinky1Pos, pinky2Pos, leftPinky1Transform);
            ApplyRotation(pinky2Pos, pinky3Pos, leftPinky2Transform);
            ApplyRotation(pinky3Pos, pinky4Pos, leftPinky3Transform);
        }
        // Right Thumb
        if (landmarksData.right_hand.ContainsKey(0) && landmarksData.right_hand.ContainsKey(1) &&
            landmarksData.right_hand.ContainsKey(2) && landmarksData.right_hand.ContainsKey(3) &&
            landmarksData.right_hand.ContainsKey(4))
        {
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);
            Vector3 thumb1Pos = new Vector3(landmarksData.right_hand[1].x, landmarksData.right_hand[1].y, landmarksData.right_hand[1].z);
            Vector3 thumb2Pos = new Vector3(landmarksData.right_hand[2].x, landmarksData.right_hand[2].y, landmarksData.right_hand[2].z);
            Vector3 thumb3Pos = new Vector3(landmarksData.right_hand[3].x, landmarksData.right_hand[3].y, landmarksData.right_hand[3].z);
            Vector3 thumb4Pos = new Vector3(landmarksData.right_hand[4].x, landmarksData.right_hand[4].y, landmarksData.right_hand[4].z);
            
            ApplyRotation(thumb1Pos, thumb2Pos, rightThumb1Transform);
            ApplyRotation(thumb2Pos, thumb3Pos, rightThumb2Transform);
            ApplyRotation(thumb2Pos, thumb3Pos, rightThumb3Transform);

        }

        // Right Index Finger
        if (landmarksData.right_hand.ContainsKey(5) && landmarksData.right_hand.ContainsKey(6) &&
            landmarksData.right_hand.ContainsKey(7) && landmarksData.right_hand.ContainsKey(8))
        {
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);
            Vector3 index1Pos = new Vector3(landmarksData.right_hand[5].x, landmarksData.right_hand[5].y, landmarksData.right_hand[5].z);
            Vector3 index2Pos = new Vector3(landmarksData.right_hand[6].x, landmarksData.right_hand[6].y, landmarksData.right_hand[6].z);
            Vector3 index3Pos = new Vector3(landmarksData.right_hand[7].x, landmarksData.right_hand[7].y, landmarksData.right_hand[7].z);
            Vector3 index4Pos = new Vector3(landmarksData.right_hand[8].x, landmarksData.right_hand[8].y, landmarksData.right_hand[8].z);
            
            ApplyRotation(index1Pos, index2Pos, rightIndex1Transform);
            ApplyRotation(index2Pos, index3Pos, rightIndex2Transform);
            ApplyRotation(index3Pos, index4Pos, rightIndex3Transform);

        }

        // Right Middle Finger
        if (landmarksData.right_hand.ContainsKey(9) && landmarksData.right_hand.ContainsKey(10) &&
            landmarksData.right_hand.ContainsKey(11) && landmarksData.right_hand.ContainsKey(12))
        {
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);
            Vector3 middle1Pos = new Vector3(landmarksData.right_hand[9].x, landmarksData.right_hand[9].y, landmarksData.right_hand[9].z);
            Vector3 middle2Pos = new Vector3(landmarksData.right_hand[10].x, landmarksData.right_hand[10].y, landmarksData.right_hand[10].z);
            Vector3 middle3Pos = new Vector3(landmarksData.right_hand[11].x, landmarksData.right_hand[11].y, landmarksData.right_hand[11].z);
            Vector3 middle4Pos = new Vector3(landmarksData.right_hand[12].x, landmarksData.right_hand[12].y, landmarksData.right_hand[12].z);
            
            ApplyRotation(middle1Pos, middle2Pos, rightMiddle1Transform);
            ApplyRotation(middle2Pos, middle3Pos, rightMiddle2Transform);
            ApplyRotation(middle3Pos, middle4Pos, rightMiddle3Transform);

        }

        // Right Ring Finger
        if (landmarksData.right_hand.ContainsKey(13) && landmarksData.right_hand.ContainsKey(14) &&
            landmarksData.right_hand.ContainsKey(15) && landmarksData.right_hand.ContainsKey(16))
        {
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);
            Vector3 ring1Pos = new Vector3(landmarksData.right_hand[13].x, landmarksData.right_hand[13].y, landmarksData.right_hand[13].z);
            Vector3 ring2Pos = new Vector3(landmarksData.right_hand[14].x, landmarksData.right_hand[14].y, landmarksData.right_hand[14].z);
            Vector3 ring3Pos = new Vector3(landmarksData.right_hand[15].x, landmarksData.right_hand[15].y, landmarksData.right_hand[15].z);
            Vector3 ring4Pos = new Vector3(landmarksData.right_hand[16].x, landmarksData.right_hand[16].y, landmarksData.right_hand[16].z);
            
            ApplyRotation(ring1Pos, ring2Pos, rightRing1Transform);
            ApplyRotation(ring2Pos, ring3Pos, rightRing2Transform);
            ApplyRotation(ring3Pos, ring4Pos, rightRing3Transform);
        }

        // Right Pinky Finger
        if (landmarksData.right_hand.ContainsKey(17) && landmarksData.right_hand.ContainsKey(18) &&
            landmarksData.right_hand.ContainsKey(19) && landmarksData.right_hand.ContainsKey(20))
        {
            Vector3 wristPos = new Vector3(landmarksData.right_hand[0].x, landmarksData.right_hand[0].y, landmarksData.right_hand[0].z);
            Vector3 pinky1Pos = new Vector3(landmarksData.right_hand[17].x, landmarksData.right_hand[17].y, landmarksData.right_hand[17].z);
            Vector3 pinky2Pos = new Vector3(landmarksData.right_hand[18].x, landmarksData.right_hand[18].y, landmarksData.right_hand[18].z);
            Vector3 pinky3Pos = new Vector3(landmarksData.right_hand[19].x, landmarksData.right_hand[19].y, landmarksData.right_hand[19].z);
            Vector3 pinky4Pos = new Vector3(landmarksData.right_hand[20].x, landmarksData.right_hand[20].y, landmarksData.right_hand[20].z);
            
            ApplyRotation(pinky1Pos, pinky2Pos, rightPinky1Transform);
            ApplyRotation(pinky2Pos, pinky3Pos, rightPinky2Transform);
            ApplyRotation(pinky3Pos, pinky4Pos, rightPinky3Transform);

        }
    }


    private void OnApplicationQuit()
    {
        udpClient.Close();
        receiveThread.Abort();
    }
}
