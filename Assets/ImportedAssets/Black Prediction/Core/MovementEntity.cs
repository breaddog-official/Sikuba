﻿using System.Collections.Generic;
using UnityEngine;
using Mirror;
using NaughtyAttributes;

namespace Black.ClientSidePrediction
{
    [DisallowMultipleComponent]
    public abstract class MovementEntity : NetworkBehaviour
    {
        public enum PredictionSync
        {
            Update,
            FixedUpdate,
            LateUpdate,
        }
        [Header("Prediction")]
        [SerializeField, BoxGroup("Prediction")] private PredictionSync sync;
        [SerializeField, BoxGroup("Prediction")] private byte defaultBuffer = 2;
        //[SerializeField, BoxGroup("Prediction")] private byte bufferSpeed = 5;

        private float nextUpdate;
        private float updateRate = 0.016f;

        private ulong currentFrame;
        private ClientInput currentInput;
        private ServerResult currentResult;
        private List<ClientInput> inputs = new List<ClientInput>();

        protected abstract ClientInput GetInput();
        public abstract void SetInput(ClientInput input);
        public abstract ServerResult GetResult();
        protected abstract void SetResult(ServerResult result);
        public abstract void ApplyMovement();

        public override void OnStartAuthority()
        {
            if (!isServer)
                if (TryGetComponent(out NetworkTransformBase transformBase))
                    transformBase.enabled = false;

            base.OnStartAuthority();
        }

        protected virtual void Start()
        {
            if (NetworkServer.active && MovementSimulation.Instance != null)
            {
                MovementSimulation.Instance.AddEntity(connectionToClient, this);
            }
        }

        protected virtual void OnDestroy()
        {
            if (NetworkServer.active && MovementSimulation.Instance != null)
            {
                MovementSimulation.Instance.RemoveEntity(this);
            }
        }

        protected virtual void Update() { if (sync == PredictionSync.Update) UpdateAction(); }
        protected virtual void FixedUpdate() { if (sync == PredictionSync.FixedUpdate) UpdateAction(); }
        protected virtual void LateUpdate() { if (sync == PredictionSync.LateUpdate) UpdateAction(); }

        private void UpdateAction()
        {
            if (Time.time >= nextUpdate)
            {
                nextUpdate = Time.time + updateRate;

                Run();
            }
        }

        private void Run()
        {
            if (!isOwned || MovementSimulation.Instance == null)
            {
                return;
            }

            currentFrame++;

            BufferInput();
            CreateInput();
            ReconciliateState();
            PredictMovement();

            MovementSimulation.Instance.SendInputToServer(currentInput);
        }

        [TargetRpc]
        public void SendResultToClient(ServerResult result)
        {
            currentResult = result;
        }

        private void BufferInput()
        {
            if (isServer)
            {
                return;
            }

            byte updateRate = MovementSimulation.Instance.UpdateRate;
            var pingBuffer = (byte)(NetworkTime.rtt / 2 * updateRate);
            var targetBuffer = (byte)(defaultBuffer + pingBuffer);

            if (currentResult.Buffer > targetBuffer)
            {
                this.updateRate = 0.022f;
            }
            else if (currentResult.Buffer < targetBuffer)
            {
                this.updateRate = 0.012f;
            }
            else
            {
                this.updateRate = 0.016f;
            }
        }

        private void CreateInput()
        {
            currentInput = GetInput();
            currentInput.Frame = currentFrame;
        }

        private void ReconciliateState()
        {
            if (isServer)
            {
                return;
            }

            SetResult(currentResult);
        }

        private void PredictMovement()
        {
            if (isServer)
            {
                return;
            }

            inputs.RemoveAll(IsObsoleteInput);
            inputs.Add(currentInput);

            for (int i = 0; i < inputs.Count; i++)
            {
                SetInput(inputs[i]);
                ApplyMovement();
            }
        }

        private bool IsObsoleteInput(ClientInput input)
        {
            return input.Frame <= currentResult.Frame;
        }
    }
}