﻿// **********************************************************************
//
// Copyright (c) 2009-2015 ZeroC, Inc. All rights reserved.
//
// **********************************************************************

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace IceBuilder
{
    public class ProjectConfiguration : IVsProjectFlavorCfg
    {
        public void Initialize(IVsCfg baseConfiguration, IVsProjectFlavorCfg innerConfiguration)
        {
            _baseConfiguration = baseConfiguration;
            _innerConfiguration = innerConfiguration;
        }

        /// <summary>
        /// Provides access to a configuration interfaces such as IVsBuildableProjectCfg2
        /// or IVsDebuggableProjectCfg.
        /// </summary>
        /// <param name="iidCfg">IID of the interface that is being asked</param>
        /// <param name="ppCfg">Object that implement the interface</param>
        /// <returns>HRESULT</returns>
        public int get_CfgType(ref Guid iidCfg, out IntPtr ppCfg)
        {
            ppCfg = IntPtr.Zero;
            if(_innerConfiguration != null)
            {
                return _innerConfiguration.get_CfgType(ref iidCfg, out ppCfg);
            }
            return VSConstants.S_OK;
        }

        /// <summary>
        /// Closes the IVsProjectFlavorCfg object.
        /// </summary>
        /// <returns></returns>
        public int Close()
        {
            int hr = _innerConfiguration.Close();

            if(_baseConfiguration != null)
            {
                if(Marshal.IsComObject(_baseConfiguration))
                {
                    Marshal.ReleaseComObject(_baseConfiguration);
                }
                _baseConfiguration = null;
            }

            if(_innerConfiguration != null)
            {
                if(Marshal.IsComObject(_innerConfiguration))
                {
                    Marshal.ReleaseComObject(_innerConfiguration);
                }
                _innerConfiguration = null;
            }
            return hr;
        }

        private IVsCfg _baseConfiguration;
        private IVsProjectFlavorCfg _innerConfiguration;
    }
}