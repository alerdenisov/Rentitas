namespace Rentitas
{
    public partial class Pool<T> where T : class, IComponent
    {

        public static Pool<T> Create<TComponent1>()
            where TComponent1 : T, new()

        {
            return new Pool<T>(
                new TComponent1()
                );

        }

        public static Pool<T> Create<TComponent1, TComponent2>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2()
                );

        }

        public static Pool<T> Create<TComponent1, TComponent2, TComponent3>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3()
                );

        }

        public static Pool<T> Create<TComponent1, TComponent2, TComponent3, TComponent4>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4()
                );

        }

        public static Pool<T> Create<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5()
                );

        }

        public static Pool<T> Create<TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19, TComponent20>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()
            where TComponent20 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19(),
                new TComponent20()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19, TComponent20, TComponent21>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()
            where TComponent20 : T, new()
            where TComponent21 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19(),
                new TComponent20(),
                new TComponent21()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19, TComponent20, TComponent21, TComponent22>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()
            where TComponent20 : T, new()
            where TComponent21 : T, new()
            where TComponent22 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19(),
                new TComponent20(),
                new TComponent21(),
                new TComponent22()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19, TComponent20, TComponent21, TComponent22,
                TComponent23>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()
            where TComponent20 : T, new()
            where TComponent21 : T, new()
            where TComponent22 : T, new()
            where TComponent23 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19(),
                new TComponent20(),
                new TComponent21(),
                new TComponent22(),
                new TComponent23()
                );

        }

        public static Pool<T> Create
            <TComponent1, TComponent2, TComponent3, TComponent4, TComponent5, TComponent6, TComponent7, TComponent8,
                TComponent9, TComponent10, TComponent11, TComponent12, TComponent13, TComponent14, TComponent15,
                TComponent16, TComponent17, TComponent18, TComponent19, TComponent20, TComponent21, TComponent22,
                TComponent23, TComponent24>()
            where TComponent1 : T, new()
            where TComponent2 : T, new()
            where TComponent3 : T, new()
            where TComponent4 : T, new()
            where TComponent5 : T, new()
            where TComponent6 : T, new()
            where TComponent7 : T, new()
            where TComponent8 : T, new()
            where TComponent9 : T, new()
            where TComponent10 : T, new()
            where TComponent11 : T, new()
            where TComponent12 : T, new()
            where TComponent13 : T, new()
            where TComponent14 : T, new()
            where TComponent15 : T, new()
            where TComponent16 : T, new()
            where TComponent17 : T, new()
            where TComponent18 : T, new()
            where TComponent19 : T, new()
            where TComponent20 : T, new()
            where TComponent21 : T, new()
            where TComponent22 : T, new()
            where TComponent23 : T, new()
            where TComponent24 : T, new()

        {
            return new Pool<T>(
                new TComponent1(),
                new TComponent2(),
                new TComponent3(),
                new TComponent4(),
                new TComponent5(),
                new TComponent6(),
                new TComponent7(),
                new TComponent8(),
                new TComponent9(),
                new TComponent10(),
                new TComponent11(),
                new TComponent12(),
                new TComponent13(),
                new TComponent14(),
                new TComponent15(),
                new TComponent16(),
                new TComponent17(),
                new TComponent18(),
                new TComponent19(),
                new TComponent20(),
                new TComponent21(),
                new TComponent22(),
                new TComponent23(),
                new TComponent24()
                );

        }
    }
}