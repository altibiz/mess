namespace Mess.System.Extensions.Functions;

public static class FunctionCastExtensions
{
  public static Func<TReturn> Cast<TCreated, TReturn>(this Func<TCreated> @this)
    where TReturn : class => () => (@this() as TReturn)!;

  public static Func<T1, TReturn> Cast<TCreated, TReturn, T1>(
    this Func<T1, TCreated> @this
  )
    where TReturn : class => (arg1) => (@this(arg1) as TReturn)!;

  public static Func<T1, T2, TReturn> Cast<TCreated, TReturn, T1, T2>(
    this Func<T1, T2, TCreated> @this
  )
    where TReturn : class => (arg1, arg2) => (@this(arg1, arg2) as TReturn)!;

  public static Func<T1, T2, T3, TReturn> Cast<TCreated, TReturn, T1, T2, T3>(
    this Func<T1, T2, T3, TCreated> @this
  )
    where TReturn : class =>
    (arg1, arg2, arg3) => (@this(arg1, arg2, arg3) as TReturn)!;

  public static Func<T1, T2, T3, T4, TReturn> Cast<
    TCreated,
    TReturn,
    T1,
    T2,
    T3,
    T4
  >(this Func<T1, T2, T3, T4, TCreated> @this)
    where TReturn : class =>
    (arg1, arg2, arg3, arg4) => (@this(arg1, arg2, arg3, arg4) as TReturn)!;

  public static Func<T1, T2, T3, T4, T5, TReturn> Cast<
    TCreated,
    TReturn,
    T1,
    T2,
    T3,
    T4,
    T5
  >(this Func<T1, T2, T3, T4, T5, TCreated> @this)
    where TReturn : class =>
    (arg1, arg2, arg3, arg4, arg5) =>
      (@this(arg1, arg2, arg3, arg4, arg5) as TReturn)!;

  public static Func<T1, T2, T3, T4, T5, T6, TReturn> Cast<
    TCreated,
    TReturn,
    T1,
    T2,
    T3,
    T4,
    T5,
    T6
  >(this Func<T1, T2, T3, T4, T5, T6, TCreated> @this)
    where TReturn : class =>
    (arg1, arg2, arg3, arg4, arg5, arg6) =>
      (@this(arg1, arg2, arg3, arg4, arg5, arg6) as TReturn)!;

  public static Func<T1, T2, T3, T4, T5, T6, T7, TReturn> Cast<
    TCreated,
    TReturn,
    T1,
    T2,
    T3,
    T4,
    T5,
    T6,
    T7
  >(this Func<T1, T2, T3, T4, T5, T6, T7, TCreated> @this)
    where TReturn : class =>
    (arg1, arg2, arg3, arg4, arg5, arg6, arg7) =>
      (@this(arg1, arg2, arg3, arg4, arg5, arg6, arg7) as TReturn)!;

  public static Func<T1, T2, T3, T4, T5, T6, T7, T8, TReturn> Cast<
    TCreated,
    TReturn,
    T1,
    T2,
    T3,
    T4,
    T5,
    T6,
    T7,
    T8
  >(this Func<T1, T2, T3, T4, T5, T6, T7, T8, TCreated> @this)
    where TReturn : class =>
    (arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) =>
      (@this(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8) as TReturn)!;
}
