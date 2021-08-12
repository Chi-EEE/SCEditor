﻿using System;
using System.Diagnostics;
using System.IO;
using System.Drawing;

namespace SCEditor.ScOld
{
    internal class ScImage : IDisposable
    {
        #region Constructors
        public ScImage()
        {
            // Space
        }
        #endregion

        #region Fields & Properties
        internal bool _disposed;
        internal ushort _imageType;
        protected ushort _width;
        protected ushort _height;
        protected Bitmap _bitmap;

        public ushort Width => _width;
        public ushort Height => _height;
        public Bitmap Image => _bitmap;

        public Bitmap GetBitmap()
        {
            return _bitmap;
        }

        public void SetHeight(ushort height)
        {
            _height = height;
        }

        public ushort GetImageType()
        {
            return _height;
        }

        public void SetImageType(ushort type)
        {
            _imageType = type;
        }

        public void SetWidth(ushort width)
        {
            _width = width;
        }

        public virtual string GetImageTypeName()
        {
            return "unknown";
        }
        #endregion

        #region Methods
        public virtual void ReadImage(uint packetID, uint packetSize, BinaryReader br)
        {
            _width = br.ReadUInt16();
            _height = br.ReadUInt16();
        }

        public virtual void WriteImage(Stream input)
        {
            input.Write(BitConverter.GetBytes(_width), 0, 2);
            input.Write(BitConverter.GetBytes(_height), 0, 2);
        }

        public void SetBitmap(Bitmap b)
        {
            _bitmap = b;
            _width = (ushort)b.Width;
            _height = (ushort)b.Height;
        }
        
        public virtual void Print()
        {
            Console.WriteLine("Width: " + _width);
            Console.WriteLine("Height: " + _height);
        }
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _bitmap.Dispose();
            }

            _disposed = true;
        }
        #endregion
    }
}