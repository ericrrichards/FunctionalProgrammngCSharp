using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace FunctionalProgrammngCSharp {
    public static class Chapter1 {
        // 2
        public static Func<T, bool> Negate<T>(this Func<T, bool> predicate) => 
            t => !predicate(t);
        
        [Test]
        public static void TestNegate() {
            Func<int, bool> pred = i => i != 1;
            Assert.True(new[]{1,1,1,1}.All(pred.Negate()));
        }
        // 3 - author's code uses an Append() method that doesn't exist...
        public static List<int> QSort(this List<int> list) {
            if (list.Count == 0) {
                return new List<int>();
            }
            var pivot = list[0];
            var rest = list.Skip(1).ToList();

            var left = rest.Where(i => i < pivot);
            var right = rest.Where(i => i >= pivot);

            return left.ToList().QSort()
                .Concat(new[] {pivot})
                .Concat(right.ToList().QSort()).ToList();
        }

        [Test]
        public static void TestQSortInt() {
            var list = new List<int> {3, 2, 1, 6, 5, 4, 7};
            var expected = new List<int>{1,2,3,4,5,6,7};
            Assert.AreEqual(expected, list.QSort());
        }

        // 4
        public static List<T> QSort<T>(this List<T> list, Comparison<T> comparer) {
            if (list.Count == 0) {
                return new List<T>();
            }
            var pivot = list[0];
            var rest = list.Skip(1).ToList();
            var left = rest.Where(t => comparer(t, pivot) < 0);
            var right = rest.Where(t => comparer(t, pivot) >= 0);

            return left.ToList().QSort(comparer)
                .Concat(new[] { pivot })
                .Concat(right.ToList().QSort(comparer)).ToList();

        }

        [Test]
        public static void TestQSortDouble() {
            var list = new List<double> {4.4, 1.1, 5.5, 2.2, 6.6, 3.3};
            var expected = new List<double> {1.1, 2.2, 3.3, 4.4, 5.5, 6.6};
            int Comparer(double d, double d1) => d == d1 ? 0 : d < d1 ? -1 : 1;
            Assert.AreEqual(expected, list.QSort(Comparer));
        }

        // 5
        public static R Using<TDisp, R>(Func<TDisp> disposable, Func<TDisp, R> f) where TDisp : IDisposable {
            using (var d = disposable()) return f(d);
        }
    }
}