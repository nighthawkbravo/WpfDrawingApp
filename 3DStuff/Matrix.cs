using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace WpfAppComputerGraphics2._3DStuff
{
    public class Matrix
    {
        private ArrayList Values;
        private int rowCount;
        private int columnCount;

        public int RowCount
        {
            get { return rowCount; }
        }
        public int ColumnCount
        {
            get { return columnCount; }
        }
        public Matrix(int m, int n)
        {
            rowCount = m;
            columnCount = n;

            Values = new ArrayList(m);

            for (int i = 0; i < m; i++)
            {
                Values.Add(new ArrayList(n));

                for (int j = 0; j < n; j++)
                {
                    ((ArrayList)Values[i]).Add(0);
                }
            }
        }

        // Creates matrix from 2-d double array.
        public Matrix(double[,] values)
        {
            if (values == null)
            {
                Values = new ArrayList();
                columnCount = 0;
                rowCount = 0;
            }

            rowCount = (int)values.GetLongLength(0);
            columnCount = (int)values.GetLongLength(1);

            Values = new ArrayList(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                Values.Add(new ArrayList(columnCount));

                for (int j = 0; j < columnCount; j++)
                {
                    ((ArrayList)Values[i]).Add(values[i, j]);
                }
            }
        }

        // Creates column vector from double array.
        public Matrix(double[] values)
        {
            if (values == null)
            {
                Values = new ArrayList();
                columnCount = 0;
                rowCount = 0;
            }

            rowCount = values.Length;
            columnCount = 1;

            Values = new ArrayList(rowCount);

            for (int i = 0; i < rowCount; i++)
            {
                Values.Add(new ArrayList(1));

                ((ArrayList)Values[i]).Add(values[i]);
            }
        }

        public virtual double this[int i, int j]
        {
            set
            {
                if (i <= 0 || j <= 0)
                    throw new ArgumentOutOfRangeException("Indices must be real positive.");

                if (i > rowCount)
                {
                    // dynamically add i-Rows new rows...
                    for (int k = 0; k < i - rowCount; k++)
                    {
                        this.Values.Add(new ArrayList(columnCount));

                        // ...with Cols columns
                        for (int t = 0; t < columnCount; t++)
                        {
                            ((ArrayList)Values[rowCount + k]).Add(0);
                        }
                    }

                    rowCount = i; // ha!
                }


                if (j > columnCount)
                {
                    // dynamically add j-Cols columns to each row
                    for (int k = 0; k < rowCount; k++)
                    {
                        for (int t = 0; t < j - columnCount; t++)
                        {
                            ((ArrayList)Values[k]).Add(0);
                        }
                    }

                    columnCount = j;
                }

                ((ArrayList)Values[i - 1])[j - 1] = value;
                //this.Values[i - 1, j - 1] = value; 
            }
            get
            {
                if (i > 0 && i <= rowCount && j > 0 && j <= columnCount)
                {
                    //Complex buf;
                    //
                    //try
                    //{
                    //    buf = (Complex)(((ArrayList)Values[i - 1])[j - 1]);
                    //}
                    //catch
                    //{
                    //    buf = new Complex((double)((int)(((ArrayList)Values[i - 1])[j - 1])));
                    //}
                    //
                    // return buf;                    

                    return (double)((ArrayList)Values[i - 1])[j - 1];
                }
                else
                    throw new ArgumentOutOfRangeException("Indices must not exceed size of matrix.");
            }
        }

        public Matrix Row(int i)
        {
            if (i <= 0 || i > rowCount)
                throw new ArgumentException("Index exceed matrix dimension.");

            //return (new Matrix((Complex[])((ArrayList)Values[i - 1]).ToArray(typeof(Complex)))).Transpose();

            Matrix buf = new Matrix(columnCount, 1);

            for (int j = 1; j <= this.columnCount; j++)
            {
                //buf[j] = this[i, j];
            }

            return buf;
        }

        public Matrix Column(int j)
        {
            Matrix buf = new Matrix(this.rowCount, 1);

            for (int i = 1; i <= this.rowCount; i++)
            {
                //buf[i] = this[i, j];
            }

            return buf;
        }

        //public static double Dot(Matrix v, Matrix w)
        //{
        //    //int m = v.VectorLength();
        //    //int n = w.VectorLength();

        //    //if (m == 0 || n == 0)
        //    //    throw new ArgumentException("Arguments need to be vectors.");
        //    //else if (m != n)
        //    //    throw new ArgumentException("Vectors must be of the same length.");

        //    //double buf = 0;

        //    //for (int i = 1; i <= m; i++)
        //    //{
        //    //    buf += v[i] * w[i];
        //    //}

        //    return buf;
        //}

        //public static Matrix operator *(Matrix A, Matrix B)
        //{

        //    if (A.ColumnCount != B.RowCount)
        //        throw new ArgumentException("Inner matrix dimensions must agree.");

        //    Matrix C = new Matrix(A.RowCount, B.ColumnCount);

        //    for (int i = 1; i <= A.RowCount; i++)
        //    {
        //        for (int j = 1; j <= B.ColumnCount; j++)
        //        {
        //            C[i, j] = Dot(A.Row(i), B.Column(j));
        //        }
        //    }

        //    return C;

        //}
    }
}
