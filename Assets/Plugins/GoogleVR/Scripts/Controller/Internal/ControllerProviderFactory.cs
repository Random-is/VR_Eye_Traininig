//-----------------------------------------------------------------------
// <copyright file="ControllerProviderFactory.cs" company="Google Inc.">
// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

using UnityEngine;

/// @cond
namespace Gvr.Internal {
    /// Factory that provides a concrete implementation of IControllerProvider for the
    /// current platform.
    static class ControllerProviderFactory {
        /// Provides a concrete implementation of IControllerProvider appropriate for the current
        /// platform. This method never returns null. In the worst case, it might return a dummy
        /// provider if the platform is not supported. For demo purposes the emulator controller
        /// is returned in the editor and in Standalone buids, for use inside the desktop player.
        static internal IControllerProvider CreateControllerProvider(GvrControllerInput owner) {
// Use emualtor in editor, and in Standalone builds (for demo purposes).
#if UNITY_EDITOR
            // Use the Editor controller provider that supports the controller emulator and the mouse.
            return new EditorControllerProvider(owner.emulatorConnectionMode);

#elif UNITY_ANDROID
            if (AndroidNativeShimControllerProvider.ShimAvailable())
            {
                // Use the GVR Unity Shim API.
                return new AndroidNativeShimControllerProvider();
            }
            else
            {
                Debug.LogWarning(
                    "GVR Unity Shim not found. Creating dummy controller provider.");
                return new DummyControllerProvider();
            }
#else
            // Platform not supported.
            Debug.LogWarning("No controller support on this platform.");
            return new DummyControllerProvider();
#endif // UNITY_EDITOR || UNITY_ANDROID
        }
    }
}

/// @endcond